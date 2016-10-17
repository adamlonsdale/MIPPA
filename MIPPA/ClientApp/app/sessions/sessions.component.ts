import { Component, OnInit, OnDestroy } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';

import { Subscription } from 'rxjs/Rx';

import { SessionsService } from './sessions.service';

@Component({
  selector: 'cp-sessions',
  template: require('./sessions.component.html')
})
export class SessionsComponent implements OnInit, OnDestroy {
    private subscription: Subscription;
    managerId: number;
  
  constructor(private sessionsService: SessionsService, private activatedRoute: ActivatedRoute) { }

  ngOnInit() {

      this.subscription =
          this.activatedRoute.params.subscribe(
              (params: any) => {
                  this.managerId = +params['managerId']

                  this.sessionsService.fetchSessions(this.managerId);
              }
          );
  }

  ngOnDestroy() {
      this.subscription.unsubscribe();
  }

  filteredCountriesSingle: any[];

  filterCountrySingle(event) {
        let query = event.query;
        this.sessionsService.getCountries().then(countries => {
            this.filteredCountriesSingle = this.filterCountry(query, countries);
        });
    }
    
    filterCountry(query, countries: any[]):any[] {
        //in a real application, make a request to a remote url with the query and return filtered results, for demo we filter at client side
        let filtered : any[] = [];
        for(let i = 0; i < countries.length; i++) {
            let country = countries[i];
            if(country.name.toLowerCase().indexOf(query.toLowerCase()) == 0) {
                filtered.push(country);
            }
        }
        return filtered;
    }
        

}
