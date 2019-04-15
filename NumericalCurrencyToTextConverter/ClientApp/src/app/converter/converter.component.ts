import { Component, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { HttpParams } from '@angular/common/http';

@Component({
  selector: 'app-converter-component',
  templateUrl: './converter.component.html'
})
export class ConverterComponent {
  public currency = "";
  public numericalCurrency = "";
  http: HttpClient;
  baseUrl: string

  constructor(http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    this.baseUrl = baseUrl;
    this.http = http;
  }

  public convert() {
    let httpParams = new HttpParams()
      .set('currency', this.numericalCurrency);
    this.http.get<ConverterResponse>(this.baseUrl + 'api/CurrencyConverter/Convert', { params: httpParams, responseType: 'json' })
      .subscribe(result => {
        if (result.status == 0) {
          this.currency = result.convertedText;
        } else {
          this.currency = result.errorMessage;
        }
    }, error => console.error(error.error));
  }
}

interface ConverterResponse {
  convertedText: string;
  errorMessage: string;
  status: number;
}
