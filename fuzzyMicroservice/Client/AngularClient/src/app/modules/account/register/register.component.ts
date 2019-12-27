import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { AccountService } from '@services/account.service';
import { first } from 'rxjs/operators';
@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.scss']
})
export class RegisterComponent implements OnInit {

  registerForm: FormGroup;
  loading = false;
  submitted = false;

  constructor(
      private formBuilder: FormBuilder,
      private router: Router,
    private userService: AccountService
      ) { }

  ngOnInit() {
      this.registerForm = this.formBuilder.group({
          firstName: ['', Validators.required],
          lastName: ['', Validators.required],
        username: ['', Validators.required],
        email: ['', Validators.required],
        confirmPassword: ['', [Validators.required, Validators.minLength(8)]],
          password: ['', [Validators.required, Validators.minLength(8)]]
      });
  }

 
  onSubmit() {
      this.submitted = true;

      // stop here if form is invalid
      if (this.registerForm.invalid) {
          return;
      }

      this.loading = true;
      this.userService.register(this.registerForm.value)
          .pipe(first())
          .subscribe(data => {
                 console.log(data)
                  this.router.navigate(['']);
              },
              error => {
                  console.error(error);
                  this.loading = false;
              });
  }

}
