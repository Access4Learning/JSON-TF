# JSON-TF

This is the playground for the JSON Task Force.  Seeking to make JSON a first class citizen in PESC and A4L specifications.

## GETTING STARTED

### Install Node and NPM

[Node](https://nodejs.org/en/) and [npm](https://www.npmjs.com/get-npm) are required to run file conversions and tests.

Install dependencies with:

```Bash
$ npm install
npm notice created a lockfile as package-lock.json. You should commit this file.
npm WARN grunt-execute@0.2.2 requires a peer of grunt@~0.4.1 but none is installed. You must install peer dependencies yourself.

added 20 packages from 451 contributors in 3.392s
```

And then install the Grunt command line:

```Bash
$ npm install -g grunt-cli
C:\Users\jon.nicholson.ZINETHQ\AppData\Roaming\npm\grunt -> C:\Users\jon.nicholson.ZINETHQ\AppData\Roaming\npm\node_modules\grunt-cli\bin\grunt
+ grunt-cli@1.2.0
updated 2 packages in 1.558s
```

### Run Conversions

```Bash
$ grunt
Running "execute:target" (execute) task
-> executing C:\dev\repos\other\JSON-TF\index.js
xLea-empty list.json was saved using Gossner Notation.
xLea-empty list.json was saved using Nicholson Notation.
xLea-missing simple content.json was saved using Gossner Notation.
xLea-missing simple content.json was saved using Nicholson Notation.
lea.json was saved using Gossner Notation.
lea.json was saved using Nicholson Notation.
xLea.json was saved using Nicholson Notation.
xLea.json was saved using Gossner Notation.
xStudents.json was saved using Nicholson Notation.
xStudents.json was saved using Gossner Notation.
-> completed C:\dev\repos\other\JSON-TF\index.js (554ms)

>> 1 file and 0 calls executed (571ms)

Done.
```

### Run Tests

```Bash
$ npm test

> json-tf@1.0.0 test /Users/sallen/Projects/Demo/JSON-TF
> mocha

  xStudentsGoessner.json
    #familyName
      ✓ should find "Lovell" as familyName using xStudentsGoessner.xStudents.xStudent.name.familyName


  1 passing (9ms)

```