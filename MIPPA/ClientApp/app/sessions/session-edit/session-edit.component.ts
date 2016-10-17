import { Component, Input, Output, EventEmitter, OnInit } from '@angular/core';

import { Session } from '../../model/session';

import { NgForm } from '@angular/forms';

@Component({
  selector: 'cp-session-edit',
  template: require('./session-edit.component.html')
})
export class SessionEditComponent implements OnInit {
  @Input() session: Session;
  @Output() onSessionSaved = new EventEmitter<Session>();
  @Output() onSessionCancel = new EventEmitter<boolean>();

  constructor() { }

  ngOnInit() {
  }

  onSubmit(form: any) {
    console.log(form);
    this.session.name = form.name;
    this.session.format = form.format;
    this.session.matchupType = form.matchup;

    this.onSessionSaved.emit(this.session);
  }

  onCancel() {
    this.onSessionCancel.emit(true);
  }

}
