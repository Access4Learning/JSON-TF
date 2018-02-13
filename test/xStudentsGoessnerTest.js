var assert = require('chai').assert;
var xStudentsGoessner = require("../Examples/xStudentsGoessner.json");

describe('xStudentsGoessner.json', function() {
  describe('#familyName', function() {
    it('should find "Lovell" as familyName using xStudentsGoessner.xStudents.xStudent.name.familyName', function() {
      assert.equal(xStudentsGoessner.xStudents.xStudent.name.familyName, 'Lovell');
    });
  });
});