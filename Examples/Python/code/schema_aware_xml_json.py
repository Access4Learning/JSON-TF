# -*- coding: utf-8 -*-
"""
Created on Wed Mar 28 15:11:52 2018

@author: MorrisM
"""

import xmlschema
import json
from datetime import datetime

sfn = '../data/jsontrans.xsd'
ifn = '../data/jsontrans.xml'
doc_request_schema = xmlschema.XMLSchema(sfn)
dict_form = doc_request_schema.to_dict(ifn)
json_form = json.dumps(dict_form, indent=2)
date = datetime.now().strftime('%y_%m_%d_%H_%M')
fd = open('../report/xmlconvert' + date + '.json', 'w')
fd.write(json_form)
fd.close()
print(json_form)