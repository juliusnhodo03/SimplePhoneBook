import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { IPhoneBook, IEntry } from '../models/phone-books.models';

import { tap } from 'rxjs/operators';
import { Observable } from 'rxjs';
import * as $ from 'jquery';

const apiUrl: string = 'http://localhost:4100/api';

@Injectable()
export class PhoneBookService {

  constructor(private http: HttpClient) {}

  getPhoneBook(): Observable<IPhoneBook> {
    return this.http.get(`${apiUrl}/PhoneBook`).pipe(
      tap((phoneBook: IPhoneBook) => {
        return phoneBook;
    }));
  }

  search(searchText: string): Observable<IEntry[]> {
    if ($.trim(searchText) === '') {
      searchText = '_none__valid_'
    }
    let url = `${apiUrl}/PhoneBook/Search/${searchText}`;
    return this.http.get(url).pipe(
      tap((contacts: IEntry[]) => {
        return contacts;
      }));
  }

}
