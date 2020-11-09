import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { IEntry } from '../models/phone-books.models';

import { tap } from 'rxjs/operators';
import { Observable } from 'rxjs';
import * as $ from 'jquery';

const phoneBookApiUrl: string = 'http://localhost:4100/api/PhoneBook';

@Injectable()
export class SearchPhoneService {

  constructor(private http: HttpClient) { }

  search(searchText: string): Observable<IEntry[]> {
    if ($.trim(searchText) === '') {
      searchText = '_none__valid_'
    }
    let url = `${phoneBookApiUrl}/Search/Contacts/${searchText}`;
    return this.http.get(url).pipe(
      tap((contacts: IEntry[]) => {
        return contacts;
    }));
  }

}
