import { Component } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { FormBuilder, Validators } from '@angular/forms';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  searchForm = this.fb.group({
    destination: ['', Validators.required],
    checkIn: ['', Validators.required],
    checkOut: ['', Validators.required],
    roomType: ['']
  });

  results: any[] = [];

  constructor(private fb: FormBuilder, private http: HttpClient) {}

  search() {
    const value = this.searchForm.value;

    // Client-side validation: require destination and ensure checkOut > checkIn
    if (!value.destination || !value.checkIn || !value.checkOut) {
      alert('Please provide destination, check-in and check-out dates.');
      return;
    }

    const checkIn = new Date(value.checkIn);
    const checkOut = new Date(value.checkOut);
    if (checkOut <= checkIn) {
      alert('Check-out must be after check-in.');
      return;
    }

    const params = new HttpParams()
      .set('destination', value.destination)
      .set('checkIn', value.checkIn)
      .set('checkOut', value.checkOut)
      .set('roomType', value.roomType || '');

    this.http.get<any[]>('https://localhost:59292/hotels/search', { params }).subscribe(data => {
      this.results = data;
      this.results?.forEach(result => {
        result.available = true;
      });
    }, err => {
      console.error('Search error', err);
      alert('Search failed: ' + (err?.error?.error ?? err?.statusText ?? 'unknown'));
    });
  }

  book(result: any) {
    const payload = {
      providerName: result.providerName,
      destination: result.destination,
      checkIn: result.checkIn,
      checkOut: result.checkOut,
      roomType: result.roomType,
      passengerName: 'Test User',
      documentType: 'Passport',
      documentNumber: 'X12345678'
    };

    this.http.post('https://localhost:59292/hotels/book', payload).subscribe(() => {
      result.available = false;
      alert('Booking submitted successfully.');
    }, err => {
      console.error('Booking error', err);
      alert('Booking failed: ' + (err?.error?.error ?? err?.statusText ?? 'unknown'));
    });
  }
}
