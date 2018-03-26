# JSON-TF
This is the playground for the JSON Task Force.  Seeking to make JSON a first class citizen in PESC and A4L specifications.

## GETTING STARTED

### Install Node and NPM
[Node](https://nodejs.org/en/) and [npm](https://www.npmjs.com/get-npm) are required to run tests.

### Run Tests
```
$ npm test
```

### Example Output
```
$ npm test

> json-tf@1.0.0 test /Users/sallen/Projects/Demo/JSON-TF
> mocha



  xStudentsGoessner.json
    #familyName
      ✓ should find "Lovell" as familyName using xStudentsGoessner.xStudents.xStudent.name.familyName


  1 passing (9ms)

```

## Existing Projects That Deal With XSD's, XML and JSON
### EdExchange
* [Github](https://github.com/jhwhetstone/cdsWebserver)
    * [pesc-sdk: Module responsible for generating JAXB classes from XSD](https://github.com/jhwhetstone/cdsWebserver/tree/master/pesc-sdk)
    * Important Git Commits
        * [Update Network Server to use PESC centric workflow](https://github.com/jhwhetstone/cdsWebserver/commit/25c80625e2af8d2d89986f27ada47e02e12090a8)
        * [Added logic to support marshalling and unmarshalling PESC documents using a JAXB library (Moxy) that support both JSON and XML.](https://github.com/jhwhetstone/cdsWebserver/commit/0c46570b895de995a3b9b7bb2ed5112327f3ec40)


## References
### JAXB and MOXy
* [Developing JAXB Applications Using EclipseLink MOXy](http://www.eclipse.org/eclipselink/documentation/2.6/moxy/toc.htm)