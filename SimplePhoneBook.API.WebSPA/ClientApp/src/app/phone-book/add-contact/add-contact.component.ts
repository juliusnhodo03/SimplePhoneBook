import { Component } from '@angular/core';
import { IEntry, IPhoneBook } from '../../shared/models/phone-books.models';
import { PhoneBookService } from '../../shared/services/phone-book.service';
import { Router } from '@angular/router';
import * as $ from 'jquery';

@Component({
  selector: 'app-add-contact',
  templateUrl: './add-contact.component.html'
})
export class AddContactComponent {
  contact: IEntry = {
    entryId: 0,
    phoneBookId: 0,
    name: null,
    phoneNumber: null
  };

  phonebook: IPhoneBook;

  constructor(private phoneBookService: PhoneBookService, private router: Router) {
    this.phoneBookService.getPhoneBook().subscribe(e => this.phonebook = e);
  }

  createContact(entry: IEntry): void {
    if ($.trim(entry.name) === '' || $.trim(entry.phoneNumber) === '') {
      alert("Please supply all details to create an entry!");
      return;
    }
    entry.phoneBookId = this.phonebook.phoneBookId;
    this.phoneBookService.addContact(entry)
      .subscribe(added =>
      {
        if (added) {
          this.router.navigate(['/phonebook']);
          return;
        }
        alert("Failed to add contact to phonebook.");
      });
  }
}
