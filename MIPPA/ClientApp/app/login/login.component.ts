import { Component, OnInit } from '@angular/core';
import { NgForm } from '@angular/forms';
import { LoginService } from './login.service';

@Component({
    selector: 'cp-login',
    template: require('./login.component.html'),
    styles: [require('./login.component.css')]
})
export class LoginComponent implements OnInit {
    username: string;
    password: string;

    loginInitiated: boolean = false;

    loginViewModel: LoginViewModel = new LoginViewModel();

    constructor(private loginService: LoginService) {
        this.loginViewModel.valid = true;
    }

    ngOnInit() {
        this.loginService.loginChanged.subscribe(
            data => {
                this.loginViewModel = data;
                this.loginInitiated = false;
            });
    }

    loginClick(data: NgForm) {
        this.loginInitiated = true;
        this.loginService.Login(data.value.username, data.value.password);
    }
}

class LoginViewModel {
    username: string;
    password: string;
    type: string;
    valid: boolean;
    id: number;
}