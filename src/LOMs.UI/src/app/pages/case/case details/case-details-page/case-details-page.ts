import { Component, inject, OnInit } from '@angular/core';
import { PageHeaderComponent } from '../../../../../shared/components/page header/page-header-component/page-header-component';
import { ActivatedRoute, RouterLink, RouterLinkActive, RouterOutlet } from '@angular/router';
import { SessionManagement } from '../../../../../core/services/session/session-management';
import { loggedUser } from '../../../../../core/services/session/models/cls-user';
import { enRole } from '../../../../../shared/enums/roles';


@Component({
  selector: 'app-case-details-page',
  imports: [PageHeaderComponent, RouterLink, RouterOutlet, RouterLinkActive],
  templateUrl: './case-details-page.html',
  styleUrl: './case-details-page.css',
})
export class CaseDetailsPage {
  user:loggedUser = inject(SessionManagement).getSession()!;
  hasRole() {
    return this.user.role === enRole.ExecutiveManager ||  this.user.role === enRole.GeneralManager;
  }
}
