import { Component, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { FormGroup, FormControl, Validators } from '@angular/forms';

declare const zoid: any;

@Component({
  selector: 'app-input-details',
  templateUrl: './input-details.component.html'
})
export class InputDetailsComponent {
  private _http: HttpClient;

  constructor(http: HttpClient) {
    this._http = http;
  }

  data: any;
  ClientForm: FormGroup;
  submitted = false;
  EventValue: any = "Save";

  ngOnInit(): void {
    this.initializeZoid();
    this.resetFrom();

    this.ClientForm = new FormGroup({
      name: new FormControl("", [Validators.required]),
      accountNo: new FormControl("", [Validators.required]),
    })
  }

  private initializeZoid() {
    const InputDetailsComponent = new zoid.create({
      tag: "zoid-input-details",
      url: "http://localhost/ModularAngularWeb/input-details",

      dimensions: {
        width: "100%",
        height: "100%"
      }

    });
  }

  private postData(formData) {
    return this._http.post('./api/inputdetails', formData);
  }

  private resetFrom() {
    //this.ClientForm.reset();
    this.EventValue = "Save";
    this.submitted = false;
  } 

  Save() {
    this.submitted = true;
    if (this.ClientForm.invalid) {
      return;
    }
    this.postData(this.ClientForm.value).subscribe((data: any[]) => {
      this.data = data;
      this.resetFrom();
    })
  }  
}
