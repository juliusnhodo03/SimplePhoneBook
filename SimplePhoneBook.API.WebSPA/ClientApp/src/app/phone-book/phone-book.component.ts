import { Component, OnInit } from '@angular/core';
import { IPhoneBook, IEntry } from '../shared/models/phone-books.models';
import { PhoneBookService } from '../shared/services/phone-book.service';
import * as _ from "lodash";

@Component({
  selector: 'app-phone-book',
  templateUrl: './phone-book.component.html'
})
export class PhoneBookComponent implements OnInit {
  phoneBook: IPhoneBook = {
    phoneBookId: 0,
    name: null,
    entries: []
  };

  loading: boolean;

  constructor(private phoneBookService: PhoneBookService) {}

  ngOnInit(): void {
    this.phoneBookService.getPhoneBook().subscribe(phoneBook => this.phoneBook = phoneBook);
  }

  searchResults(contacts: IEntry[]): void {
    this.phoneBook.entries = _.cloneDeep(contacts);
  }
}
