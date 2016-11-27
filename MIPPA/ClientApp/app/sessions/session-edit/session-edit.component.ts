import { Component, Input, Output, EventEmitter, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

import { Session } from '../../model/session';

import { NgForm } from '@angular/forms';

import { Subscription } from 'RxJs';

@Component({
    selector: 'cp-session-edit',
    template: require('./session-edit.component.html')
})
export class SessionEditComponent implements OnInit {
    private subscription: Subscription;

    @Input() session: Session;
    managerId: number;
    @Output() onSessionSaved = new EventEmitter<Session>();
    @Output() onSessionCancel = new EventEmitter<boolean>();

    constructor(private activatedRoute: ActivatedRoute) { }

    ngOnInit() {
        this.subscription =
            this.activatedRoute.params.subscribe(
                (params: any) => {
                    this.managerId = +params['managerId'];
                }
            );
    }

    onSubmit(form: any) {
        console.log(form);
        this.session.name = form.name;
        this.session.format = form.format;
        this.session.matchupType = form.matchup;
        this.session.managerId = this.managerId;

        this.onSessionSaved.emit(this.session);
    }

    onCancel() {
        this.onSessionCancel.emit(true);
    }

}
