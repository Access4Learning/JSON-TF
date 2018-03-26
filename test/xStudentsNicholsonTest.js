var assert = require('chai').assert;
var xStudentsNicholson = require("../Examples/xStudentsNicholson.json");

describe('xStudentsNicholson.json', function() {
    describe('#familyName', function() {
        it('should find "Lovell" as familyName using xStudentsNicholson.xStudents.xStudent[0].name.familyName._', function() {
            assert.equal(xStudentsNicholson.xStudents.xStudent[0].name.familyName._, 'Lovell');
        });
    });
});