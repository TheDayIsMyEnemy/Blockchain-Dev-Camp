var bitcoin = require('bitcoinjs-lib');

var tx = new bitcoin.TransactionBuilder();

// Add the input (who is paying):
// [previous transaction hash, index of the output to use]
var txId = 'aa94ab02c182214f090e99a0d57021caffd0f195a81c24602b1028b130b63e31';
tx.addInput(txId,0);

// Add the output (who to pay to):
// [payee's address, amount in satoshis]
tx.addOutput("1Gokm82v6DmtwKEB8AiVhm82hyFSsEvBDK", 15000);

// Initialize a private key using WIF
var privateKeyWIF = 'L1uyy5qTuGrVXrmrsvHWHgVzW9kKdrp27wBC7Vs6nZDTF2BRUVwy';
var keyPair = bitcoin.ECPair.fromWIF(privateKeyWIF);

// Sign the first input with the new key
tx.sign(0, keyPair)

// Print transaction serialized as hex
console.log(tx.build().toHex())