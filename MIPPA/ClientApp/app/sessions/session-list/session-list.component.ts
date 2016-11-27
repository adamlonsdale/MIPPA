import { Component, OnInit } from '@angular/core';

import { Session } from '../../model/session';

import { SessionsService } from '../sessions.service';

@Component({
    selector: 'cp-session-list',
    template: require('./session-list.component.html')
})
export class SessionListComponent implements OnInit {
    sessions: Session[];

    // For adding new sessions to list
    newSession: Session = null;
    addingSession: boolean;


    constructor(private sessionsService: SessionsService) { }

    ngOnInit() {
        this.sessions = this.sessionsService.getSessions();
        this.sessionsService.sessionsChanged.subscribe(
            (sessions: Session[]) => this.sessions = sessions
        );
    }

    onAdd() {
        this.newSession = new Session(0, 0, '', 0, '', [], null);
        this.addingSession = true;
    }

    onSessionSaved(session: Session) {
        if (this.addingSession) {
            this.addingSession = false;
            this.sessionsService.addSession(session);
        }
    }

    onEditingSession(session: Session) {
        this.sessionsService.editSession(session);
    }

    onSessionCancel() {
        this.addingSession = false;
    }

}
