import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { IPhoneBook, IEntry } from '../models/phone-books.models';

import { tap } from 'rxjs/operators';
import { Observable } from 'rxjs';

const phoneBookApiUrl: string = 'http://localhost:4100/api/PhoneBook';

@Injectable()
export class PhoneBookService {

  constructor(private http: HttpClient) {}

  getPhoneBook(): Observable<IPhoneBook> {
    return this.http.get(`${phoneBookApiUrl}/Contacts`).pipe(
      tap((phoneBook: IPhoneBook) => {
        return phoneBook;
    }));
  }


  addContact(contact: IEntry): Observable<boolean> {
    let options = this.getHeaders();

    return this.http.post(`${phoneBookApiUrl}/Add/Contact`, contact, options).pipe(
      tap((added: boolean) => {
        return added;
    }));
  }


  private getHeaders() {
    const httpOptions =
    {
      headers: new HttpHeaders({
        'Content-Type': 'application/json'
      })
    };
    return httpOptions;
  }
}
