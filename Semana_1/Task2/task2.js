const { sha3_256 } = require('js-sha3'), fs = require('fs'), path = require('path');
function calcularSHA3_256(data){
    return sha3_256(data);
}
email = "malp18774@gmail.com"
d=path.join(__dirname, 'data');
a=fs.readdirSync(d).filter(f=>f.endsWith('.data'));
const hashes = a.map(f => {
    const filePath = path.join(d, f);
    const data = fs.readFileSync(filePath);
    const hash = calcularSHA3_256(data);
    return hash;
});
hashes.sort((a, b) => b.localeCompare(a)); 
hashesUnidos = hashes.join('');
const stringFinal = hashesUnidos + email; 
hashFinal = calcularSHA3_256(stringFinal);
console.log(hashFinal);