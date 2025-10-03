import { Routes } from '@angular/router';
import { Login } from './pages/auth/login/login/login';
import { HomeComponent } from './pages/home/home-component/home-component';
import { AddCaseComponent } from './pages/case/add-case/add-case-page/add-case-page';
import { MangeEmployeePage } from './pages/employee/mange-employee-page/mange-employee-page/mange-employee-page';
import { courtResolverResolver } from './pages/case/resolvers/court-resolver-resolver';
import { employeeNamesResolver } from './pages/case/resolvers/employee/employee-names-resolver';
import { CasesListPage } from './pages/case/cases list/cases-list-page/cases-list-page/cases-list-page';
import { CaseDetailsPage } from './pages/case/case details/case-details-page/case-details-page';
import { CaseDetailsComponent } from './pages/case/case details/components/case-details-component/case-details-component';
import { caseDetailsResolverResolver } from './pages/case/resolvers/case/case-details-resolver-resolver';
import { CaseContract } from './pages/case/case details/components/case-contract/case-contract';
import { CasePoa } from './pages/case/case details/components/case-poa/case-poa';
import { CaseAttachments } from './pages/case/case details/components/case-attachments/case-attachments';
import { App } from './app';
import { authGuard } from '../core/guards/authGuard/auth-guard';
import { CaseSessions } from './pages/case/case details/components/case-sesstions/case-sesstions';
import { DraftCasesPage } from './pages/case/draft-cases/draft-cases-page/draft-cases-page';
import { CaseJudgmentsComponents } from './pages/case/case details/components/case-judgments-components/case-judgments-components';
import { JudgmentsListPage } from './pages/case/judgments/judgments-list-page/judgments-list-page';
import { TemplatesPage } from './pages/case/template/templats-page/templats-page';
import { enRole } from '../shared/enums/roles';
import { childAuthGuardGuard } from '../core/guards/children-auth/child-auth-guard-guard';
import { UnauthorizedPage } from '../shared/components/unatuhrized/unauthorized-page/unauthorized-page';

export const routes: Routes = [
  {
    path: '',
    // canActivate: [authGuard],
    // canActivateChild: [childAuthGuardGuard],
    children: [
      { path: '', redirectTo: 'login', pathMatch: 'full' },
      { path: 'login', component: Login },
    ],
  },
  {
    path: 'office',
    canActivate: [authGuard],
    loadComponent: () =>
      import('././layouts/user-layout/user-layout').then((c) => c.UserLayout),
    canActivateChild: [childAuthGuardGuard],
    data: {
      roles: [
        enRole.GeneralManager,
        enRole.ExecutiveManager,
        enRole.Consultant,
        enRole.Lawyer,
      ],
    },

    children: [
      { path: '', redirectTo: 'add-case', pathMatch: 'full' },
      { path: 'home', component: App },
      {
        path: 'add-case',
        component: AddCaseComponent,
        resolve: {
          court: courtResolverResolver,
          employeeNames: employeeNamesResolver,
        },

        data: {
          roles: [
            enRole.GeneralManager,
            enRole.ExecutiveManager,
            enRole.Consultant,
            enRole.Lawyer,
          ],
        },
      },
      {
        path: 'employees',
        data: { roles: [enRole.GeneralManager, enRole.ExecutiveManager] },
        component: MangeEmployeePage,
      },
      {
        path: 'case-details/:caseId',
        component: CaseDetailsPage,
        children: [
          {
            path: '',
            redirectTo: 'case',
            pathMatch: 'full',
          },
          {
            path: `case`,
            component: CaseDetailsComponent,
            resolve: {
              caseDetails: caseDetailsResolverResolver,
            },

            data: {
              roles: [
                enRole.GeneralManager,
                enRole.ExecutiveManager,
                enRole.Consultant,
                enRole.Lawyer,
              ],
            },
          },
          {
            path: `contracts`,
            component: CaseContract,

            data: {
              roles: [enRole.GeneralManager, enRole.ExecutiveManager],
            },
          },
          {
            path: `poas`,
            component: CasePoa,

            data: {
              roles: [
                enRole.GeneralManager,
                enRole.ExecutiveManager,
                enRole.Consultant,
                enRole.Lawyer,
              ],
            },
          },

          {
            path: `attachments`,
            component: CaseAttachments,

            data: {
              roles: [
                enRole.GeneralManager,
                enRole.ExecutiveManager,
                enRole.Consultant,
                enRole.Lawyer,
              ],
            },
          },

          {
            path: `sessions`,
            component: CaseSessions,

            data: {
              roles: [
                enRole.GeneralManager,
                enRole.ExecutiveManager,
                enRole.Consultant,
                enRole.Lawyer,
              ],
            },
          },
          {
            path: `judgments`,
            component: CaseJudgmentsComponents,

            data: {
              roles: [
                enRole.GeneralManager,
                enRole.ExecutiveManager,
                enRole.Consultant,
                enRole.Lawyer,
              ],
            },
          },
        ],
      },
      {
        path: 'cases-list',
        component: CasesListPage,
        resolve: {
          court: courtResolverResolver,
        },
        data: {
          roles: [
            enRole.GeneralManager,
            enRole.ExecutiveManager,
            enRole.Consultant,
            enRole.Lawyer,
          ],
        },
      },
      {
        path: 'draft-cases',
        component: DraftCasesPage,
        data: {
          roles: [
            enRole.GeneralManager,
            enRole.ExecutiveManager,
            enRole.Consultant,
            enRole.Lawyer,
          ],
        },
      },

      {
        path: 'judgments-list',
        component: JudgmentsListPage,
        data: {
          roles: [
            enRole.GeneralManager,
            enRole.ExecutiveManager,
            enRole.Consultant,
            enRole.Lawyer,
          ],
        },
      },
      {
        path: 'templates',
        component: TemplatesPage,
        data: {
          roles: [
            enRole.GeneralManager,
            enRole.ExecutiveManager,
            enRole.Consultant,
            enRole.Lawyer,
          ],
        },
      },
    ],
  },

  {
    path: 'unauthorized',
    component: UnauthorizedPage,
  },
];
