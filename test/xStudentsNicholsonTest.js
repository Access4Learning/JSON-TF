var assert = require('chai').assert;
var xStudentsNicholson = require("../Examples/xStudentsNicholson.json");

describe('xStudentsNicholson.json', function() {
    describe('#familyName', function() {
        it('should find "Lovell" as familyName using xStudentsNicholson.xStudents.xStudent.name.familyName_', function() {
            assert.equal(xStudentsNicholson.xStudents.xStudent.name.familyName._, 'Lovell');
        });
    });
});