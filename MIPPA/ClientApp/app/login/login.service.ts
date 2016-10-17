import { Injectable, EventEmitter } from '@angular/core';
import { Http, Headers, Response } from '@angular/http';
import { Router } from '@angular/router';

import 'rxjs/Rx';

@Injectable()
export class LoginService {
    private headers: Headers;
    loginChanged = new EventEmitter<any>();

    constructor(private http: Http, private router: Router) {
        this.headers = new Headers();
        this.headers.append('Content-Type', 'application/json');
        this.headers.append('Accept', 'application/json');
    }

    public Login(userName: string, password: string) {
        let loginViewModel: any = null;

        // Using the username and password, check to see if valid user
        this.http.post('/api/login',
            JSON.stringify(
                {
                    username: userName,
                    password: password
                }),
            { headers: this.headers })
            .map(
            (response: Response) => response.json())
            .subscribe(
            data => {
                loginViewModel = data;

                console.log(loginViewModel);

                if (loginViewModel.valid &&
                    loginViewModel.id >= 0 &&
                    loginViewModel.type == "manager") {
                    this.router.navigate(['/', 'app', 'administration', loginViewModel.id, 'sessions']);
                } else if (
                    loginViewModel.valid &&
                    loginViewModel.id >= 0 &&
                    loginViewModel.type == "team") {

                    if (loginViewModel.mode == 'edit') {
                        this.router.navigate(['/', 'app', 'scorecard', loginViewModel.id], { queryParams: { 'edit': 'true' } });
                    }
                    else if (loginViewModel.mode == 'view') {
                        this.router.navigate(['/', 'app', 'scorecard', loginViewModel.id], { queryParams: { 'view': 'true' } });
                    }
                } else {
                    this.loginChanged.emit(loginViewModel);
                }
            }
            );
    }
}