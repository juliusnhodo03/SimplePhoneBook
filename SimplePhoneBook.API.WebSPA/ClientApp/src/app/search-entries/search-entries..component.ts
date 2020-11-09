import { Component, OnInit, ElementRef, Output, EventEmitter } from '@angular/core';

import { IEntry } from '../shared/models/phone-books.models';
import { SearchPhoneService } from '../shared/services/search.phone-book.service';

import { Observable } from 'rxjs';
import 'rxjs/add/observable/fromEvent';
import 'rxjs/add/operator/debounceTime';
import 'rxjs/add/operator/do';
import 'rxjs/add/operator/filter';
import 'rxjs/add/operator/map';
import 'rxjs/add/operator/switch';

@Component({
  selector: 'app-search-entries',
  templateUrl: './search-entries.component.html'
})
export class SearchEntriesComponent implements OnInit {
  @Output() loading: EventEmitter<boolean> = new EventEmitter<boolean>();
  @Output() contacts: EventEmitter<IEntry[]> = new EventEmitter<IEntry[]>();

  constructor(
    private searchPhoneService: SearchPhoneService,
    private el: ElementRef) { }


  public ngOnInit(): void {
    Observable.fromEvent(this.el.nativeElement, 'keyup')
      .map((e: any) => e.target.value)
      // only once every 350ms
      .debounceTime(350)
      .do(() => this.loading.emit(true))
      .map((query: string) => this.searchPhoneService.search(query))
      .switch()
      .subscribe((contacts: IEntry[]) => {
        // on success
        this.loading.emit(false);
        this.contacts.emit(contacts);
      },
        (err: any) => {
          // on error
          this.loading.emit(false);
        },
        () => {
          // on completion
          this.loading.emit(false);
        });
  }


}
