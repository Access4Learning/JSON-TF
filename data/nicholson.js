/**
 * Copyright 2018 Jon Nicholson (www.drjonnicholson.com)
 * Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
 *
 * The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
 *
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
 */

function xml2json(xml) {
    // Knowledge of what are list elements, and what elements they contain
    var lists = {
        // listNodeName: itemNodeName
        'externalIdList': 'externalId',
        'phoneNumberList': 'phoneNumber',
        'addressRefIdList': 'addressRefId',
        'otherPhoneNumbers': 'phoneNumber',
        'otherEmails': 'email'
    };

    var toObj = function (xml) {
        var o = {},
            i,
            n,
            item,
            hasElementChild = false;

        for (i = 0; i < xml.attributes.length; i++) {
            o['_' + xml.attributes[i].nodeName] = (xml.attributes[i].nodeValue || '').toString();
        }

        for (n = xml.firstChild; n; n = n.nextSibling) {
            if (n.nodeType === 1) {
                hasElementChild = true;
                break;
            }
        }

        if (lists[xml.nodeName]) {
            // Expecting a list
            item = lists[xml.nodeName];
            o[item] = [];

            for (n = xml.firstChild; n; n = n.nextSibling) {
                if (n.nodeType === 1) {
                    if (n.nodeName !== item) {
                        throw 'Unexpected element ' + n.nodeName + ' in ' + xml.nodeName + ', expected ' + item;
                    }

                    o[item].push(toObj(n));
                }
            }
        } else if (hasElementChild) {
            for (n = xml.firstChild; n; n = n.nextSibling) {
                // handles multiple occurrences that aren't in a list, but order is lost
                item = n.nodeName;
                if (n.nodeType === 1) {
                    if (o[item]) {
                        if (o[item] instanceof Array) {
                            o[item].push(toObj(n));
                        } else {
                            o[item] = [o[item], toObj(n)];
                        }
                    } else {
                        o[item] = toObj(n);
                    }
                }
            }
        } else {
            o._ = xml.textContent.trim() || null;
        }

        return o;
    };

    var json = {};
    xml = ((xml.nodeType === 9) ? xml.documentElement : xml);
    xml.normalize();
    json[xml.nodeName] = toObj(xml);
    return JSON.stringify(json, null, '  ');
}