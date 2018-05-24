# -*- coding: utf-8 -*-
"""
Created on Wed Mar 28 15:11:52 2018

@author: MorrisM
"""

import xmlschema
import json
from datetime import datetime
from xml.etree import ElementTree



def create_converter():
    return xmlschema.converters.XMLSchemaConverter(
            namespaces=None, 
            dict_class=None,
            list_class=None, text_key='value',
            attr_prefix='', cdata_prefix=None,
            )


sfn = '../data/jsontrans.xsd'
ifn = '../data/jsontrans.xml'
doc_request_schema = xmlschema.XMLSchema(sfn)
tree = ElementTree.parse(ifn)
root = tree.getroot()
print(root.tag)
dict_form = doc_request_schema.to_dict(ifn, decimal_type=float,converter=create_converter())
json_form = json.dumps(dict_form, indent=2)
date = datetime.now().strftime('%y_%m_%d_%H_%M')
fd = open('../reports/xmlconvert' + date + '.json', 'w')
fd.write(json_form)
fd.close()
print(json_form)