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
    { type: 'maxlength', message: 'Email cannot be more than 100 characters' },
    { type: 'pattern', message: 'Enter a valid email' }
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
  gender: [{ type: 'required', message: 'Gender is required' }],
  dateOfBirth: [{ type: 'required', message: 'Date of Birth is required' }],
  role: [
    { type: 'required', message: 'Role is required' },
    { type: 'required', message: 'Please select a valid role' }
  ],
  cardNumber: [
    { type: 'pattern', message: 'Please enter a valid card number' },
    { type: 'required', message: 'Card number is required' },
    {
      type: 'maxlength',
      message: 'Card number cannot be more than 18 digits'
    }
  ],
  description: [{ type: 'maxlength', message: 'description cannot be more than 250 characters' }],
  password: [
    { type: 'required', message: 'Password is required' },
    {
      type: 'minlength',
      message: 'Password must be at least 4 characters long'
    }
  ]
};

export const libraryAssetValidationMessages = {
  title: [
    { type: 'required', message: 'Title is required' },
    { type: 'maxlength', message: 'Title cannot be more than 25 characters' }
  ],
  author: [
    { type: 'required', message: 'Author is required' },
    {
      type: 'maxlength',
      message: 'Author cannot be more than 25 characters'
    },
    { type: 'authorError', message: 'Please select a valid author' }
  ],
  year: [
    { type: 'required', message: 'Year is required' },
    { type: 'pattern', message: 'Please enter a valid year' }
  ],
  numberOfCopies: [{ type: 'required', message: 'Number of copies is required' }],
  description: [
    { type: 'required', message: 'Description is required' },
    {
      type: 'maxlength',
      message: 'description cannot be more than 500 characters'
    }
  ],
  categoryId: [{ type: 'required', message: 'Category is required' }],
  assetTypeId: [{ type: 'required', message: 'Type is required' }],
  isbn: [{ type: 'required', message: 'ISBN is required' }]
};
