/**
 * Copyright 2018 Jon Nicholson (www.drjonnicholson.com)
 * Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
 *
 * The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
 *
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
 */

var fs = require('fs');
var DOMParser = require('xmldom').DOMParser;

function gossner(file) {
    fs.readFile('./data/in/' + file, 'utf8', function (err, data) {
        eval(fs.readFileSync('./data/gossner.js').toString());

        if (err) {
            console.error(err);
            return;
        }

        data = new DOMParser().parseFromString(data, 'text/xml');

        file = file.replace('.xml', '.json');

        fs.writeFile('./data/out/gossner/' + file, xml2json(data, '  '), function (err) {
            if (err) {
                console.error(err);
                return;
            }
            console.log(file + ' was saved using Gossner Notation.');
        });
    });
}

function nicholson(file) {
    fs.readFile('./data/in/' + file, 'utf8', function (err, data) {
        eval(fs.readFileSync('./data/nicholson.js').toString());

        if (err) {
            console.error(err);
            return;
        }

        data = new DOMParser().parseFromString(data, 'text/xml');

        file = file.replace('.xml', '.json');

        fs.writeFile('./data/out/nicholson/' + file, xml2json(data), function (err) {
            if (err) {
                console.error(err);
                return;
            }
            console.log(file + ' was saved using Nicholson Notation.');
        });
    });
}


fs.readdir('./data/in', (err, files) => {
    files.forEach(file => {
        gossner(file);
        nicholson(file);
    });
})