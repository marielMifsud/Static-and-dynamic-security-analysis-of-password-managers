"use strict";
var __importDefault = (this && this.__importDefault) || function (mod) {
    return (mod && mod.__esModule) ? mod : { "default": mod };
};
Object.defineProperty(exports, "__esModule", { value: true });
exports.getOtp = exports.getPassword = exports.selectCredential = exports.selectCredentials = void 0;
const clipboard_1 = require("@napi-rs/clipboard");
const otplib_1 = require("otplib");
const { cli } = require("winston/lib/winston/config");
const winston_1 = __importDefault(require("winston"));
const crypto_1 = require("../crypto");
const utils_1 = require("../utils");
const decryptPasswordTransactions = async (db, transactions, secrets) => {
    const authentifiantTransactions = transactions.filter((transaction) => transaction.type === 'AUTHENTIFIANT');
    const passwordsDecrypted = await Promise.all(authentifiantTransactions.map((transaction) => (0, crypto_1.decryptTransaction)(transaction, secrets)));
    return passwordsDecrypted;
};
const selectCredentials = async (params) => {
    const { secrets, filters, db } = params;
    winston_1.default.debug(`Retrieving: ${filters && filters.length > 0 ? filters.join(' ') : ''}`);
    const transactions = db
        .prepare(`SELECT * FROM transactions WHERE login = ? AND action = 'BACKUP_EDIT'`)
        .bind(secrets.login)
        .all();
    
    const credentialsDecrypted = await decryptPasswordTransactions(db, transactions, secrets);
    console.log(await decryptPasswordTransactions(db, transactions, secrets));
    console.log(">>> Database: ", db);
    console.log(">>> Secrets: ", secrets);
    console.log(">>> Transections: ", transactions);
    // transform entries [{_attributes: {key:xx}, _cdata: ww}] into an easier-to-use object
    const beautifiedCredentials = credentialsDecrypted.map((item) => Object.fromEntries(item.root.KWAuthentifiant.KWDataItem.map((entry) => [
        entry._attributes.key[0].toLowerCase() + entry._attributes.key.slice(1),
        entry._cdata,
    ])));
    let matchedCredentials = beautifiedCredentials;
    if (filters?.length) {
        const parsedFilters = [];
        filters.forEach((filter) => {
            const [splitFilterKey, ...splitFilterValues] = filter.split('=');
            const filterValue = splitFilterValues.join('=') || splitFilterKey;
            const filterKeys = splitFilterValues.length > 0 ? splitFilterKey.split(',') : ['url', 'title'];
            const canonicalFilterValue = filterValue.toLowerCase();
            parsedFilters.push({
                keys: filterKeys,
                value: canonicalFilterValue,
            });
        });
        matchedCredentials = matchedCredentials?.filter((item) => parsedFilters
            .map((filter) => filter.keys.map((key) => item[key]?.toLowerCase().includes(filter.value)))
            .flat()
            .some((b) => b));
    }
    return matchedCredentials;
};
exports.selectCredentials = selectCredentials;
const selectCredential = async (params, onlyOtpCredentials = false) => {
    let matchedCredentials = await (0, exports.selectCredentials)(params);
    if (onlyOtpCredentials) {
        matchedCredentials = matchedCredentials.filter((credential) => credential.otpSecret);
    }
    if (!matchedCredentials || matchedCredentials.length === 0) {
        throw new Error('No credential with this name found');
    }
    else if (matchedCredentials.length === 1) {
        return matchedCredentials[0];
    }
    return (0, utils_1.askCredentialChoice)({ matchedCredentials, hasFilters: Boolean(params.filters?.length) });
};
exports.selectCredential = selectCredential;
const getPassword = async (params) => {
    const clipboard = new clipboard_1.Clipboard();
    const selectedCredential = await (0, exports.selectCredential)(params);
    switch (params.output || 'clipboard') {
        case 'clipboard':
            clipboard.setText(selectedCredential.password);
            
           function getText()
           {
            console.log(">> clipboard after 3 minutes: ", clipboard.getText());
           }
            console.log(`🔓 Password for "${selectedCredential.title || selectedCredential.url || 'N\\C'}" copied to clipboard!`);
            console.log(">>> Password copied: ", selectedCredential.password);
            console.log(">>> Title: ", selectedCredential.title);
            console.log(">>> Username: ", selectedCredential.login);
            console.log(">>> URL: ", selectedCredential.url);
            if (selectedCredential.otpSecret) {
                const token = otplib_1.authenticator.generate(selectedCredential.otpSecret);
                const timeRemaining = otplib_1.authenticator.timeRemaining();
                console.log(`🔢 OTP code: ${token} \u001B[3m(expires in ${timeRemaining} seconds)\u001B[0m`);
            }

            const myTimeout = setTimeout(getText, 180000);
            break;
        case 'password':
            console.log(selectedCredential.password);
            break;
        default:
            throw new Error('Unable to recognize the output mode.');
    }
};
exports.getPassword = getPassword;
const getOtp = async (params) => {
    const clipboard = new clipboard_1.Clipboard();
    const selectedCredential = await (0, exports.selectCredential)(params, true);
    // otpSecret can't be null because onlyOtpCredentials is set to true above
    // eslint-disable-next-line @typescript-eslint/no-non-null-assertion
    const token = otplib_1.authenticator.generate(selectedCredential.otpSecret);
    const timeRemaining = otplib_1.authenticator.timeRemaining();
    switch (params.output || 'clipboard') {
        case 'clipboard':
            clipboard.setText(token);
            console.log(`🔢 OTP code: ${token} \u001B[3m(expires in ${timeRemaining} seconds)\u001B[0m`);
            break;
        case 'otp':
            console.log(token);
            break;
        default:
            throw new Error('Unable to recognize the output mode.');
    }
};
exports.getOtp = getOtp;
