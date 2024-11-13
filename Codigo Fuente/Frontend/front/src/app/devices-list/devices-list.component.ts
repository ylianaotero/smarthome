import { Component } from '@angular/core';

interface Device {
  name: string;
  model: string;
  photoUrl: string;
  companyName: string;
}

@Component({
  selector: 'app-devices-list',
  templateUrl: './devices-list.component.html',
  styleUrls: ['./devices-list.component.css']
})


export class DevicesListComponent {
  devices: Device[] = [
    {
      name: 'Window Alarm Sensors 4 Pack',
      model: 'abcdef',
      photoUrl: 'https://m.media-amazon.com/images/I/81d3gWM2VHL._AC_SL1500_.jpg',
      companyName: 'IoT Home'
    },
    {
      name: 'Ring Stick Up Cam Battery',
      model: 'abcdef',
      photoUrl: 'https://m.media-amazon.com/images/I/419BrDcflML.jpg',
      companyName: 'IoT Home'
    },
    {
      name: 'Window Alarm Sensors 4 Pack',
      model: 'abcdef',
      photoUrl: 'https://m.media-amazon.com/images/I/81d3gWM2VHL._AC_SL1500_.jpg',
      companyName: 'IoT Home'
    },
    {
      name: 'Ring Stick Up Cam Battery',
      model: 'abcdef',
      photoUrl: 'https://m.media-amazon.com/images/I/419BrDcflML.jpg',
      companyName: 'IoT Home'
    },
    {
      name: 'Window Alarm Sensors 4 Pack',
      model: 'abcdef',
      photoUrl: 'https://m.media-amazon.com/images/I/81d3gWM2VHL._AC_SL1500_.jpg',
      companyName: 'IoT Home'
    },
    {
      name: 'Ring Stick Up Cam Battery',
      model: 'abcdef',
      photoUrl: 'https://m.media-amazon.com/images/I/419BrDcflML.jpg',
      companyName: 'IoT Home'
    },
    {
      name: 'Window Alarm Sensors 4 Pack',
      model: 'abcdef',
      photoUrl: 'https://m.media-amazon.com/images/I/81d3gWM2VHL._AC_SL1500_.jpg',
      companyName: 'IoT Home'
    },
    {
      name: 'Ring Stick Up Cam Battery',
      model: 'abcdef',
      photoUrl: 'https://m.media-amazon.com/images/I/419BrDcflML.jpg',
      companyName: 'IoT Home'
    },
    {
      name: 'Window Alarm Sensors 4 Pack',
      model: 'abcdef',
      photoUrl: 'https://m.media-amazon.com/images/I/81d3gWM2VHL._AC_SL1500_.jpg',
      companyName: 'IoT Home'
    },
    {
      name: 'Ring Stick Up Cam Battery',
      model: 'abcdef',
      photoUrl: 'https://m.media-amazon.com/images/I/419BrDcflML.jpg',
      companyName: 'IoT Home'
    },
    {
      name: 'Window Alarm Sensors 4 Pack',
      model: 'abcdef',
      photoUrl: 'https://m.media-amazon.com/images/I/81d3gWM2VHL._AC_SL1500_.jpg',
      companyName: 'IoT Home'
    },
    {
      name: 'Ring Stick Up Cam Battery',
      model: 'abcdef',
      photoUrl: 'https://m.media-amazon.com/images/I/419BrDcflML.jpg',
      companyName: 'IoT Home'
    },
    {
      name: 'Window Alarm Sensors 4 Pack',
      model: 'abcdef',
      photoUrl: 'https://m.media-amazon.com/images/I/81d3gWM2VHL._AC_SL1500_.jpg',
      companyName: 'IoT Home'
    },
    {
      name: 'Ring Stick Up Cam Battery',
      model: 'abcdef',
      photoUrl: 'https://m.media-amazon.com/images/I/419BrDcflML.jpg',
      companyName: 'IoT Home'
    },
    {
      name: 'Window Alarm Sensors 4 Pack',
      model: 'abcdef',
      photoUrl: 'https://m.media-amazon.com/images/I/81d3gWM2VHL._AC_SL1500_.jpg',
      companyName: 'IoT Home'
    },
    {
      name: 'Ring Stick Up Cam Battery',
      model: 'abcdef',
      photoUrl: 'https://m.media-amazon.com/images/I/419BrDcflML.jpg',
      companyName: 'IoT Home'
    },
    {
      name: 'Window Alarm Sensors 4 Pack',
      model: 'abcdef',
      photoUrl: 'https://m.media-amazon.com/images/I/81d3gWM2VHL._AC_SL1500_.jpg',
      companyName: 'IoT Home'
    },
    {
      name: 'Ring Stick Up Cam Battery',
      model: 'abcdef',
      photoUrl: 'https://m.media-amazon.com/images/I/419BrDcflML.jpg',
      companyName: 'IoT Home'
    },
  ];

  doNothing() {
    console.log('Doing nothing');
  }

  selectedPhotoUrl: string = '';

  openModal(photoUrl: string): void {
    this.selectedPhotoUrl = photoUrl;
  }
}
