import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { RouterModule } from '@angular/router';

import { AppComponent } from './app.component';
import { NavMenuComponent } from './nav-menu/nav-menu.component';
import { HomeComponent } from './home/home.component';
import { PhoneBookComponent } from './phone-book/phone-book.component';
import { SearchEntriesComponent } from './search-entries/search-entries..component';
import { AddContactComponent } from './phone-book/add-contact/add-contact.component';
import { EditContactComponent } from './phone-book/edit-contact/edit-contact.component';
import { DeleteContactComponent } from './phone-book/delete-contact/delete-contact.component';

import { PhoneBookService } from './shared/services/phone-book.service';
import { ContactsService } from './shared/services/contacts.service';

@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent,
    HomeComponent,
    SearchEntriesComponent,
    PhoneBookComponent,
    AddContactComponent,
    EditContactComponent,
    DeleteContactComponent,
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    FormsModule,
    RouterModule.forRoot([
      { path: '', component: HomeComponent, pathMatch: 'full' },
      { path: 'phonebook', component: PhoneBookComponent },
      { path: 'phonebook/contact/create', component: AddContactComponent },
      { path: 'phonebook/contact/edit/:id', component: EditContactComponent },
      { path: 'phonebook/contact/delete/:id', component: DeleteContactComponent },
    ])
  ],
  providers: [PhoneBookService, ContactsService],
  bootstrap: [AppComponent]
})
export class AppModule { }
