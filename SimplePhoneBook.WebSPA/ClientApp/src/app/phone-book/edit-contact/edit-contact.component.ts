import { Component } from '@angular/core';
import { IEntry, IPhoneBook } from '../../shared/models/phone-books.models';
import { Router, ActivatedRoute } from '@angular/router';
import * as $ from 'jquery';
import { ContactsService } from '../../shared/services/contacts.service';

@Component({
  selector: 'app-edit-contact',
  templateUrl: './edit-contact.component.html'
})
export class EditContactComponent {
  contact: IEntry = {
      entryId: 0,
      phoneBookId: 0,
      name: null,
      phoneNumber: null
  };

  phonebook: IPhoneBook;

  constructor(
    private contactsService: ContactsService,
    private router: Router,
  private currentRoute: ActivatedRoute)
  {
      const id = +this.currentRoute.snapshot.params["id"];
      this.contactsService.getContact(id).subscribe(e => this.contact = e);
  }

  updateContact(entry: IEntry): void {
      if ($.trim(entry.name) === '' || $.trim(entry.phoneNumber) === '') {
          alert("Please supply all details to edit an entry!");
          return;
      }
      this.contactsService.updateContact(entry)
          .subscribe(updated =>
          {
              if (updated) {
                  this.router.navigate(['/phonebook']);
                  return;
              }
              alert("Failed to edit entry in phonebook.");
          });
  }

}
