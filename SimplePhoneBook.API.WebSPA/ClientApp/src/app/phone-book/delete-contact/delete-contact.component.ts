import { Component } from '@angular/core';
import { IEntry, IPhoneBook } from '../../shared/models/phone-books.models';
import { Router, ActivatedRoute } from '@angular/router';
import * as $ from 'jquery';
import { ContactsService } from '../../shared/services/contacts.service';

@Component({
  selector: 'app-delete-contact',
  templateUrl: './delete-contact.component.html'
})
export class DeleteContactComponent {
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

  deleteContact(id: number): void {
    this.contactsService.deleteContact(id)
        .subscribe(deleted =>
        {
            if (deleted) {
                this.router.navigate(['/phonebook']);
                return;
            }
            alert("Failed to delete entry in phonebook.");
        });
  }

}
