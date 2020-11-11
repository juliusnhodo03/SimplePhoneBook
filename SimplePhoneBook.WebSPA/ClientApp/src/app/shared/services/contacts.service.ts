import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { IEntry } from '../models/phone-books.models';

import { tap } from 'rxjs/operators';
import { Observable } from 'rxjs';

const apiUrl: string = 'http://localhost:4100/api';

@Injectable()
export class ContactsService {

  constructor(private http: HttpClient) { }

  getContact(id: number): Observable<IEntry> {
    return this.http.get(`${apiUrl}/Contacts/${id}`).pipe(
      tap((phoneBook: IEntry) => {
        return phoneBook;
      }));
  }

  addContact(contact: IEntry): Observable<boolean> {
    let options = this.getHeaders();
    return this.http.post(`${apiUrl}/Contacts`, contact, options).pipe(
      tap((added: boolean) => {
        return added;
      }));
  }

  updateContact(contact: IEntry): Observable<boolean> {
    let options = this.getHeaders();
    return this.http.put(`${apiUrl}/Contacts`, contact, options).pipe(
      tap((updated: boolean) => {
        return updated;
      }));
  }

  deleteContact(id: number): Observable<boolean> {
    let options = this.getHeaders();
    return this.http.delete(`${apiUrl}/Contacts/${id}`, options).pipe(
      tap((deleted: boolean) => {
        return deleted;
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
