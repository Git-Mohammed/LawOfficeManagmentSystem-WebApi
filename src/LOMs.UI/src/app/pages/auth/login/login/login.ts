import { Component, OnInit } from '@angular/core';
import {
  FormControl,
  FormGroup,
  FormsModule,
  NonNullableFormBuilder,
  ReactiveFormsModule,
} from '@angular/forms';
import { AuthManagement } from '../../../../../core/services/auth/auth-management';
import { Router } from '@angular/router';
import { ToasterService } from '../../../../../core/services/toaster-service';
import { SessionManagement } from '../../../../../core/services/session/session-management';

export interface loginCredtial {
  natId: FormControl<string>;
  password: FormControl<string>;
}

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [FormsModule, ReactiveFormsModule],
  templateUrl: './login.html',
  styleUrl: './login.css',
})
export class Login implements OnInit{
  loginForm: FormGroup<loginCredtial>;
  showErrors: boolean = false;
  constructor(
    private authService: AuthManagement,
    private sessionManagement:SessionManagement,
    private route: Router,
    private toastService: ToasterService,
    private fb: NonNullableFormBuilder
  ) {
    this.loginForm = this.fb.group({
      natId: this.fb.control(''),
      password: this.fb.control(''),
    });
  }
  ngOnInit(): void {
    if (this.sessionManagement.isAuthenticated())
    {
      this.route.navigate(['/office'])
    }
  }
  login() {
    this.showErrors = true;
    if (!this.loginForm.invalid) {
      this.authService
        .login(
          this.loginForm.controls.natId.value.trim(),
          this.loginForm.controls.password.value.trim()
        )
        .subscribe({
          next: () => {
            this.route.navigate(['office']);
          },
          error: () => {
            this.toastService.error('كلمة المرور او اسم المستخدم غير صحيحن');
          },
        });
    }
  }
}
