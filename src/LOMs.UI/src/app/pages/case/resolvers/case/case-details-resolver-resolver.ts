import { ActivatedRoute, ResolveFn } from '@angular/router';
import { Observable } from 'rxjs';
import { ICseGeneralDetails } from '../../case details/components/case-details-component/case-details-component';
import { inject } from '@angular/core';
import { CaseService } from '../../services/case-service';

export const caseDetailsResolverResolver: ResolveFn<Observable<ICseGeneralDetails>> = (route, state) => {
  const id = route.parent?.paramMap.get('caseId');
  console.log(`enetenred the case details resolver and the id value is ${id}`);
  return inject(CaseService).getCaeDetails(id ? id : '');
};