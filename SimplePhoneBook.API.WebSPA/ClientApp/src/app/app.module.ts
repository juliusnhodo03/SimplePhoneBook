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

import { PhoneBookService } from './shared/services/phone-book.service';
import { SearchPhoneService } from './shared/services/search.phone-book.service';

@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent,
    HomeComponent,
    SearchEntriesComponent,
    PhoneBookComponent,
    AddContactComponent
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    FormsModule,
    RouterModule.forRoot([
      { path: '', component: HomeComponent, pathMatch: 'full' },
      { path: 'phonebook', component: PhoneBookComponent },
      { path: 'phonebook/add-contact', component: AddContactComponent },
    ])
  ],
  providers: [PhoneBookService, SearchPhoneService],
  bootstrap: [AppComponent]
})
export class AppModule { }
