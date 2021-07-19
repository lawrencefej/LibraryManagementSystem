export const validationMessages = {
  firstName: [
    { type: 'required', message: 'First Name is required' },
    {
      type: 'maxlength',
      message: 'First Name cannot be more than 25 characters'
    }
  ],
  lastName: [
    { type: 'required', message: 'Last Name is required' },
    {
      type: 'maxlength',
      message: 'Last Name cannot be more than 25 characters'
    }
  ],
  email: [
    { type: 'required', message: 'Email is required' },
    { type: 'email', message: 'Please enter a valid Email' },
    { type: 'maxlength', message: 'Email cannot be more than 100 characters' }
  ],
  phoneNumber: [
    { type: 'required', message: 'Phone Number is required' },
    { type: 'maxlength', message: 'Phone number cannot be more than 15 characters' }
  ],
  street: [
    { type: 'required', message: 'Address is required' },
    {
      type: 'maxlength',
      message: 'Address cannot be more than 50 characters'
    }
  ],
  state: [
    { type: 'required', message: 'State is required' },
    { type: 'stateValidator', message: 'Please select a valid state' }
  ],
  city: [
    { type: 'required', message: 'City is required' },
    { type: 'maxlength', message: 'City cannot be more than 25 characters' }
  ],
  zipcode: [
    { type: 'required', message: 'Zipcode is required' },
    { type: 'pattern', message: 'Please enter a valid Zipcode' }
  ],
  gender: [{ type: 'required', message: 'Gender is required' }]
};
