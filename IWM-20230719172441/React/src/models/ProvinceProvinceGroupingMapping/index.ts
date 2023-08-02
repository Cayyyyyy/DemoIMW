import { ProvinceProvinceGroupingMapping } from './ProvinceProvinceGroupingMapping'
import nameof from 'ts-nameof.macro';
import { ObjectField,  } from 'react3l-decorators';

import { Province } from 'models/Province';
import { ProvinceGrouping } from 'models/ProvinceGrouping';

ObjectField(Province)(ProvinceProvinceGroupingMapping.prototype, nameof(ProvinceProvinceGroupingMapping.prototype.province));
ObjectField(ProvinceGrouping)(ProvinceProvinceGroupingMapping.prototype, nameof(ProvinceProvinceGroupingMapping.prototype.provinceGrouping));

export * from './ProvinceProvinceGroupingMapping';
export * from './ProvinceProvinceGroupingMappingFilter';
