var assert = require('chai').assert;
var CollegeTranscript_v1_6_jaxb_moxy = require("../Examples/CollegeTranscript_v1_6_jaxb_moxy.json");

describe('CollegeTranscript_v1_6_jaxb_moxy.json', function() {
    describe('#Student First Name', function() {
        it('should find "John" as Student First Name using CollegeTranscript_v1_6_jaxb_moxy["ColTrn.CollegeTranscript"].Student.Person.Name.FirstName', function() {
            assert.equal(CollegeTranscript_v1_6_jaxb_moxy["ColTrn.CollegeTranscript"].Student.Person.Name.FirstName, 'John');
        });
    });
});