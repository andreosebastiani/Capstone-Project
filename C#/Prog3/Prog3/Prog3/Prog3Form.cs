using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;

namespace Prog3
{
    public partial class Prog3Form : Form
    {
        private List<Address> addressList; // The list of addresses
        private List<Parcel> parcelList;   // The list of parcels, though only Letters now

        // Precondition:  None
        // Postcondition: The form's GUI is prepared for display. A few test addresses are
        //                added to the list of addresses
        public Prog3Form()
        {
            InitializeComponent();

            addressList = new List<Address>();
            parcelList = new List<Parcel>();
        }

        // Precondition:  File, About menu item activated
        // Postcondition: Information about author displayed in dialog box
        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show(String.Format("Program 3{0}By: Andreo Sebastiani{0}" +
                "CIS 200{0}Summer 2015", Environment.NewLine), "About Program 3");
        }

        // Precondition:  File, Exit menu item activated
        // Postcondition: The application is exited
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        // Precondition:  Insert, Address menu item activated
        // Postcondition: The Address dialog box is displayed. If data entered
        //                are OK, an Address is created and added to the list
        //                of addresses
        private void addressToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddressForm addressForm = new AddressForm(); // The address dialog box form
            DialogResult result = addressForm.ShowDialog(); // Show form as dialog and store result

            if (result == DialogResult.OK) // Only add if OK
            {
                try
                {
                    Address newAddress = new Address(addressForm.AddressName, addressForm.Address1,
                        addressForm.Address2, addressForm.City, addressForm.State,
                        int.Parse(addressForm.ZipText)); // Use form's properties to create address
                    addressList.Add(newAddress);
                }
                catch (FormatException) // This should never happen if form validation works!
                {
                    MessageBox.Show("Problem with Address Validation!", "Validation Error");
                }
            }

            addressForm.Dispose(); // Best practice for dialog boxes
        }

        // Precondition:  Report, List Addresses menu item activated
        // Postcondition: The list of addresses is displayed in the addressResultsTxt
        //                text box
        private void listAddressesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            StringBuilder result = new StringBuilder(); // Holds text as report being built
            // StringBuilder more efficient than String

            result.Append("Addresses:");
            result.Append(Environment.NewLine); // Remember, \n doesn't always work in GUIs
            result.Append(Environment.NewLine);

            foreach (Address a in addressList)
            {
                result.Append(a.ToString());
                result.Append(Environment.NewLine);
                result.Append(Environment.NewLine);
            }

            reportTxt.Text = result.ToString();

            // Put cursor at start of report
            reportTxt.Focus();
            reportTxt.SelectionStart = 0;
            reportTxt.SelectionLength = 0;
        }

        // Precondition:  Insert, Letter menu item activated
        // Postcondition: The Letter dialog box is displayed. If data entered
        //                are OK, a Letter is created and added to the list
        //                of parcels
        private void letterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LetterForm letterForm; // The letter dialog box form
            DialogResult result;   // The result of showing form as dialog

            if (addressList.Count < LetterForm.MIN_ADDRESSES) // Make sure we have enough addresses
            {
                MessageBox.Show("Need " + LetterForm.MIN_ADDRESSES + " addresses to create letter!",
                    "Addresses Error");
                return;
            }

            letterForm = new LetterForm(addressList); // Send list of addresses
            result = letterForm.ShowDialog();

            if (result == DialogResult.OK) // Only add if OK
            {
                try
                {
                    // For this to work, LetterForm's combo boxes need to be in same
                    // order as addressList
                    Letter newLetter = new Letter(addressList[letterForm.OriginAddressIndex],
                        addressList[letterForm.DestinationAddressIndex],
                        decimal.Parse(letterForm.FixedCostText)); // Letter to be inserted
                    parcelList.Add(newLetter);
                }
                catch (FormatException) // This should never happen if form validation works!
                {
                    MessageBox.Show("Problem with Letter Validation!", "Validation Error");
                }
            }

            letterForm.Dispose(); // Best practice for dialog boxes
        }

        // Precondition:  Report, List Parcels menu item activated
        // Postcondition: The list of parcels is displayed in the parcelResultsTxt
        //                text box
        private void listParcelsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            StringBuilder result = new StringBuilder(); // Holds text as report being built
            // StringBuilder more efficient than String
            decimal totalCost = 0;                      // Running total of parcel shipping costs

            result.Append("Parcels:");
            result.Append(Environment.NewLine); // Remember, \n doesn't always work in GUIs
            result.Append(Environment.NewLine);

            foreach (Parcel p in parcelList)
            {
                result.Append(p.ToString());
                result.Append(Environment.NewLine);
                result.Append(Environment.NewLine);
                totalCost += p.CalcCost();
            }

            result.Append("------------------------------");
            result.Append(Environment.NewLine);
            result.Append(String.Format("Total Cost: {0:C}", totalCost));

            reportTxt.Text = result.ToString();

            // Put cursor at start of report
            reportTxt.Focus();
            reportTxt.SelectionStart = 0;
            reportTxt.SelectionLength = 0;
        }

        // Precondition:  File, Save Addresses menu item activated
        // Postcondition: The list of addresses is saved to a file using
        //                object serialization
        private void saveAddressesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            BinaryFormatter formatter = new BinaryFormatter(); // Object for serializing list of addresses in binary format
            FileStream output = null;                          // Stream for writing to a file
            DialogResult result;                               // Result of file dialog box
            string fileName;                                   // Name of file to save data

            using (SaveFileDialog fileChooser = new SaveFileDialog()) // Create Save File Dialog
            {
                fileChooser.CheckFileExists = false; // let user create file

                // retrieve the result of the dialog box
                result = fileChooser.ShowDialog();
                fileName = fileChooser.FileName; // get specified file name
            } // end using

            // ensure that user clicked "OK"
            if (result == DialogResult.OK)
            {

                // show error if user specified invalid file
                if (fileName == string.Empty)
                    MessageBox.Show("Invalid File Name", "Error",
                       MessageBoxButtons.OK, MessageBoxIcon.Error);
                else
                {
                    // save file via FileStream if user specified valid file
                    try
                    {
                        // open file with write access, Create will overwrite existing file
                        output = new FileStream(fileName, FileMode.Create, FileAccess.Write);

                        formatter.Serialize(output, addressList); // Serialize entire list!
                    } // end try
                    // handle exception if there is a problem opening the file
                    catch (IOException)
                    {
                        // notify user if file could not be opened
                        MessageBox.Show("Error Writing to File", "Error",
                           MessageBoxButtons.OK, MessageBoxIcon.Error);
                    } // end catch
                    // notify user if error occurs in serialization
                    catch (SerializationException)
                    {
                        MessageBox.Show("Error Writing to File", "Error",
                           MessageBoxButtons.OK, MessageBoxIcon.Error);
                    } // end catch
                    finally
                    {
                        if (output != null)
                            output.Close(); // close FileStream
                    }
                } // end else
            } // end if
        }

        // Precondition:  File, Open Addresses menu item activated
        // Postcondition: The list of addresses is read in from a file using
        //                object deserialization, replacing the existing list
        private void openAddressesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            BinaryFormatter reader = new BinaryFormatter(); // Object for deserializing list of addresses in binary format
            FileStream input = null;                        // Stream for reading from a file
            DialogResult result;                            // Result of file dialog box
            string fileName;                                // Name of file to save data
            List<Address> temp;                             // Temporary holder for list of addresses

            using (OpenFileDialog fileChooser = new OpenFileDialog()) // Create Open Dialog box
            {
                result = fileChooser.ShowDialog();
                fileName = fileChooser.FileName; // get specified name
            } // end using

            // ensure that user clicked "OK"
            if (result == DialogResult.OK)
            {
                // show error if user specified invalid file
                if (fileName == string.Empty)
                    MessageBox.Show("Invalid File Name", "Error",
                       MessageBoxButtons.OK, MessageBoxIcon.Error);
                else
                {
                    // create FileStream to obtain read access to file
                    try
                    {

                        input = new FileStream(fileName, FileMode.Open, FileAccess.Read);

                        // get list of addresses from file
                        temp = (List<Address>)reader.Deserialize(input);

                        addressList = temp; // Separated in case deserialization failed
                        parcelList.Clear(); // Clear list of parcels, since new addresses loaded
                    } // end try

                    // handle exception if there is a problem opening the file
                    catch (IOException)
                    {
                        // notify user if file could not be opened
                        MessageBox.Show("Error Reading From File", "Error",
                           MessageBoxButtons.OK, MessageBoxIcon.Error);
                    } // end catch

                    catch (SerializationException)
                    {
                        MessageBox.Show("Error Reading From File", "Error",
                           MessageBoxButtons.OK, MessageBoxIcon.Error);
                    } // end catch
                    finally
                    {
                        if (input != null)
                            input.Close(); // close FileStream
                    }
                } // end else
            } // end if
        }

        // Precondition:  Edit, Address menu item activated
        // Postcondition: The address selected from the list has been edited
        //                with the new information replacing the existing object's
        //                properties
        private void addressToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (addressList.Count > 0) // Only edit if there are addresses!
            {
                ChooseAddressForm chooseAddForm = new ChooseAddressForm(addressList); // The choose address dialog box form
                DialogResult result = chooseAddForm.ShowDialog();                     // Show form as dialog and store result

                if (result == DialogResult.OK) // Only edit if OK
                {
                    int editIndex; // Index of address to edit
                    editIndex = chooseAddForm.AddressIndex;

                    if (editIndex >= 0) // -1 if didn't select item from combo box
                    {
                        Address editAddress = addressList[editIndex]; // The address being edited
                        AddressForm addressForm = new AddressForm();  // The address dialog box form

                        // Populate form fields from selected address
                        addressForm.AddressName = editAddress.Name;
                        addressForm.Address1 = editAddress.Address1;
                        addressForm.Address2 = editAddress.Address2;
                        addressForm.City = editAddress.City;
                        addressForm.State = editAddress.State;
                        addressForm.ZipText = String.Format("{0:D5}", editAddress.Zip);

                        result = addressForm.ShowDialog(); // Show form as dialog and store result

                        if (result == DialogResult.OK) // Only edit if OK
                        {
                            // Edit address properties using form fields
                            editAddress.Name = addressForm.AddressName;
                            editAddress.Address1 = addressForm.Address1;
                            editAddress.Address2 = addressForm.Address2;
                            editAddress.City = addressForm.City;
                            editAddress.State = addressForm.State;
                            try
                            {
                                editAddress.Zip = int.Parse(addressForm.ZipText);
                            }

                            catch (FormatException) // This should never happen if form validation works!
                            {
                                MessageBox.Show("Problem with Zip Validation!", "Validation Error");
                            }
                        }
                        addressForm.Dispose(); // Best practice for dialog boxes
                    }
                }
                chooseAddForm.Dispose(); // Best practice for dialog boxes
            }
            else
                MessageBox.Show("No addresses to edit!", "No Addresses");
        }
    }
}