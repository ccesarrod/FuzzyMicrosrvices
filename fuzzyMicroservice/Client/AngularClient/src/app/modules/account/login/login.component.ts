import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { AccountService } from '@services/account.service';
import { Router, ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent implements OnInit {
  loginForm: FormGroup;
  loading = false;
  submitted = false;
  returnUrl: string = '';
  constructor(private formBuilder: FormBuilder,
    private loginService: AccountService,
    private router: Router,
    private route: ActivatedRoute) { }

  ngOnInit() {
    this.loginForm = this.formBuilder.group({
      userName: ['', Validators.required],
      password: ['', Validators.required]
    })

    this.returnUrl = this.route.snapshot.queryParams['returnUrl'] || '/'
  }

  onSubmit() { 
    this.submitted = true;
    // stop here if form is invalid
    if (this.loginForm.invalid) {
      return;
    }

    this.loading = true;
    this.loginService.login(this.loginForm.controls.userName.value, this.loginForm.controls.password.value)
      .subscribe(
        data => {
          if (data) {         
            this.returnUrl = this.route.snapshot.queryParams['returnUrl'] || '/';
            this.router.navigate([this.returnUrl]);
          }

        },
        error => {
          console.error(error);
          this.loading = false;
        });
  }

}
