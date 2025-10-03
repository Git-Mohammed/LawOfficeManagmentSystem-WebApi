import { Component, computed, inject, signal } from '@angular/core';
import { Router, RouterModule } from '@angular/router';
import { NgxSpinnerComponent } from 'ngx-spinner';
import { AuthManagement } from '../../../core/services/auth/auth-management';
import {
  MatSidenav,
  MatSidenavContainer,
  MatSidenavContent,
  MatSidenavModule,
} from '@angular/material/sidenav';
import { NgClass } from '@angular/common';
import { MatToolbarModule } from '@angular/material/toolbar';
import { PageHeaderComponent } from '../../../shared/components/page header/page-header-component/page-header-component';
import { loggedUser } from '../../../core/services/session/models/cls-user';
import { SessionManagement } from '../../../core/services/session/session-management';
import { enRole } from '../../../shared/enums/roles';
import { MatMenuModule } from '@angular/material/menu';
import { ChangePasswordDialog } from '../../pages/employee/dialogs/change-password/change-password-dialog/change-password-dialog';
import { MatDialog } from '@angular/material/dialog';

@Component({
  selector: 'app-user-layout',
  imports: [
    NgxSpinnerComponent,
    RouterModule,
    MatSidenavContainer,
    MatSidenav,
    MatSidenavContent,
    NgClass,
    MatToolbarModule,
    PageHeaderComponent,
    MatMenuModule
  ],
  templateUrl: './user-layout.html',
  styleUrl: './user-layout.css',
})
export class UserLayout {
hasRole(navElements: INavBtn) {
  return navElements.roles.some(role => this.user.role == role)
}

    private dialogof: MatDialog = inject(MatDialog);
  navElements: INavBtn[] = [
    {
      name: 'إضافة قضية جديدة',
      icon: 'fa-solid fa-plus',
      title: 'إضافة قضية',
      route: 'add-case',
      roles: [enRole.GeneralManager, enRole.ExecutiveManager, enRole.Lawyer, enRole.Consultant]
    },
    {
      name: 'القضايا المستلمة',
      icon: 'fa-solid fa-sheet-plastic',
      title: 'القضايا المستلمة',
      route: 'cases-list',
      roles: [enRole.GeneralManager, enRole.ExecutiveManager, enRole.Lawyer, enRole.Consultant]
    },
    {
      name: 'صكوك الاحكام',
      icon: 'fa-solid fa-gavel',
      title: 'صكوك الاحكام',
      route: 'judgments-list',
      roles: [enRole.GeneralManager, enRole.ExecutiveManager, enRole.Lawyer, enRole.Consultant]
    },
    {
      name: 'القضايا المسودة',
      icon: 'fa-brands fa-firstdraft',
      title: 'القضايا المسودة',
      route: 'draft-cases',
      roles: [enRole.GeneralManager, enRole.ExecutiveManager, enRole.Lawyer, enRole.Consultant]
    },
    {
      name: 'النماذج',
      icon: 'fa-solid fa-box',
      title: 'النماذج',
      route: 'templates',
      roles: [enRole.GeneralManager, enRole.ExecutiveManager, enRole.Lawyer, enRole.Consultant]
    },
    {
      name: 'إدارة الموظفين',
      icon: 'fa-solid fa-users',
      title: 'إدارة الموظفين',
      route: 'employees',
      roles: [enRole.GeneralManager, enRole.ExecutiveManager]
    },
  ];
  user:loggedUser = inject(SessionManagement).getSession()!;
  activeRout:INavBtn = this.navElements[5]
  IsnavigatorsTextVisable = computed(() => !this.collapsed());
  collapsed = signal(false);
  sideNavWidth = computed(() => (this.collapsed() ? '80px' : '255px'));
  constructor(private authMangment: AuthManagement, private route: Router) {}
  logout() {
    this.authMangment.logout().subscribe({
      next: () => {
        this.route.navigate(['/login']);
      },
      error: () => {},
    });
  }

  routeChanged(isActive:boolean, element:INavBtn) {
    if (isActive) this.activeRout = element;
  }

  changePassword() {
    this.dialogof.open(ChangePasswordDialog, {
      // height: '450px',
      minWidth: '400px',
    });
  }


}

export interface INavBtn {
  name: string;
  icon: string;
  title: string;
  route: string;
  roles:enRole[]
}
