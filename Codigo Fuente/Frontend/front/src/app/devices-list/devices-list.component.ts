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

  modalImage: string | null = null;
  isModalOpen = false;

  // Open modal and set image
  openModal(imageUrl: string) {
    this.modalImage = imageUrl;
    this.isModalOpen = true;
  }

  // Close modal
  closeModal() {
    this.isModalOpen = false;
  }

  doNothing() {
    // Do nothing
  }
}
