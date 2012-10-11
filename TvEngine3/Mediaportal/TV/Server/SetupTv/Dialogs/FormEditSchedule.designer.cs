using System.Windows.Forms;
using Mediaportal.TV.Server.SetupControls.UserInterfaceControls;

namespace Mediaportal.TV.Server.SetupTV.Dialogs
{
  partial class FormEditSchedule
  {
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    /// Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing)
    {
      if (disposing && (components != null))
      {
        components.Dispose();
      }
      base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
      this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.mpButtonSave = new MPButton();
      this.mpButtonCancel = new MPButton();
      this.checkBoxMonday = new System.Windows.Forms.CheckBox();
      this.groupBox4 = new System.Windows.Forms.GroupBox();
      this.label9 = new System.Windows.Forms.Label();
      this.mpNumericTextBoxStartingAroundDeviation = new MPNumericTextBox();
      this.dateTimePickerStartingBetweenTo = new System.Windows.Forms.DateTimePicker();
      this.label6 = new System.Windows.Forms.Label();
      this.dateTimePickerStartingBetweenFrom = new System.Windows.Forms.DateTimePicker();
      this.label2 = new System.Windows.Forms.Label();
      this.dateTimePickerStartingAround = new System.Windows.Forms.DateTimePicker();
      this.label25 = new System.Windows.Forms.Label();
      this.dateTimePickerOnDate = new System.Windows.Forms.DateTimePicker();
      this.mpLabel2 = new MPLabel();
      this.checkBoxSunday = new System.Windows.Forms.CheckBox();
      this.checkBoxTuesday = new System.Windows.Forms.CheckBox();
      this.checkBoxSaturday = new System.Windows.Forms.CheckBox();
      this.checkBoxWednesday = new System.Windows.Forms.CheckBox();
      this.checkBoxFriday = new System.Windows.Forms.CheckBox();
      this.checkBoxThursday = new System.Windows.Forms.CheckBox();
      this.groupBox5 = new System.Windows.Forms.GroupBox();
      this.mpButtonCreditAdd = new MPButton();
      this.mpButtonCreditRemove = new MPButton();
      this.mpTextBoxPerson = new MPTextBox();
      this.mpComboBoxRoles = new MPComboBox();
      this.listBoxCredits = new System.Windows.Forms.CheckedListBox();
      this.groupBox1 = new System.Windows.Forms.GroupBox();
      this.listBoxPrograms = new System.Windows.Forms.ListBox();
      this.mpComboBoxOperators = new MPComboBox();
      this.mpButtonAddProgramCondition = new MPButton();
      this.mpButtonRemoveProgramSchedule = new MPButton();
      this.mpTextBoxProgramValue = new MPTextBox();
      this.mpComboBoxProgramFields = new MPComboBox();
      this.groupBox2 = new System.Windows.Forms.GroupBox();
      this.radioOnAllChannels = new MPRadioButton();
      this.mpButtonAddAllChannelCondition = new MPButton();
      this.radioNotOnChannels = new MPRadioButton();
      this.radioOnChannels = new MPRadioButton();
      this.mpComboBoxChannels = new MPComboBox();
      this.mpButtonAddChannelCondition = new MPButton();
      this.mpButtonRemoveChannelCondition = new MPButton();
      this.mpComboBoxChannelsGroup = new MPComboBox();
      this.listBoxChannels = new System.Windows.Forms.ListBox();
      this.groupBox3 = new System.Windows.Forms.GroupBox();
      this.mpRadioButtonInAllCategories = new MPRadioButton();
      this.mpButtonAddAllCategoryCondition = new MPButton();
      this.mpRadioButtonNotInCategory = new MPRadioButton();
      this.mpRadioButtonInCategory = new MPRadioButton();
      this.mpButtonAddCategoryCondition = new MPButton();
      this.mpButtonRemoveCategoryCondition = new MPButton();
      this.mpComboBoxCategories = new MPComboBox();
      this.listBoxCategories = new System.Windows.Forms.CheckedListBox();
      this.groupBox6 = new System.Windows.Forms.GroupBox();
      this.label1 = new System.Windows.Forms.Label();
      this.checkBoxSkipRepeats = new System.Windows.Forms.CheckBox();
      this.checkBoxNewTitles = new System.Windows.Forms.CheckBox();
      this.checkBoxOnlyNewEpisodes = new System.Windows.Forms.CheckBox();
      this.groupBox7 = new System.Windows.Forms.GroupBox();
      this.mpNumericTextBoxPostRec = new MPNumericTextBox();
      this.mpNumericTextBoxPreRec = new MPNumericTextBox();
      this.label8 = new System.Windows.Forms.Label();
      this.mpNumericTextBoxPriority = new MPNumericTextBox();
      this.label7 = new System.Windows.Forms.Label();
      this.mpComboBoxKeepMethods = new MPComboBox();
      this.mpTextBoxScheduleName = new MPTextBox();
      this.label5 = new System.Windows.Forms.Label();
      this.label4 = new System.Windows.Forms.Label();
      this.label3 = new System.Windows.Forms.Label();
      this.groupBox8 = new System.Windows.Forms.GroupBox();
      this.listView2 = new System.Windows.Forms.ListView();
      this.columnHeader6 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.columnHeader7 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.columnHeader8 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.columnHeader9 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.columnHeader10 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.columnHeader11 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.columnHeader12 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.columnHeader13 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.columnHeader14 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.columnHeader15 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.columnHeader16 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.columnHeader17 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.columnHeader18 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.columnHeader19 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.mpButtonSaveTemplate = new MPButton();
      this.mpButtonApplyTemplate = new MPButton();
      this.mpComboBoxTemplates = new MPComboBox();
      this.groupBox4.SuspendLayout();
      this.groupBox5.SuspendLayout();
      this.groupBox1.SuspendLayout();
      this.groupBox2.SuspendLayout();
      this.groupBox3.SuspendLayout();
      this.groupBox6.SuspendLayout();
      this.groupBox7.SuspendLayout();
      this.groupBox8.SuspendLayout();
      this.SuspendLayout();
      // 
      // columnHeader1
      // 
      this.columnHeader1.Width = 175;
      // 
      // mpButtonSave
      // 
      this.mpButtonSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.mpButtonSave.Location = new System.Drawing.Point(968, 456);
      this.mpButtonSave.Name = "mpButtonSave";
      this.mpButtonSave.Size = new System.Drawing.Size(98, 23);
      this.mpButtonSave.TabIndex = 1;
      this.mpButtonSave.Text = "Save schedule";
      this.mpButtonSave.UseVisualStyleBackColor = true;
      this.mpButtonSave.Click += new System.EventHandler(this.mpButtonSave_Click);
      // 
      // mpButtonCancel
      // 
      this.mpButtonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.mpButtonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      this.mpButtonCancel.Location = new System.Drawing.Point(1072, 456);
      this.mpButtonCancel.Name = "mpButtonCancel";
      this.mpButtonCancel.Size = new System.Drawing.Size(75, 23);
      this.mpButtonCancel.TabIndex = 6;
      this.mpButtonCancel.Text = "Cancel";
      this.mpButtonCancel.UseVisualStyleBackColor = true;
      this.mpButtonCancel.Click += new System.EventHandler(this.mpButtonCancel_Click);
      // 
      // checkBoxMonday
      // 
      this.checkBoxMonday.AutoSize = true;
      this.checkBoxMonday.Location = new System.Drawing.Point(70, 39);
      this.checkBoxMonday.Name = "checkBoxMonday";
      this.checkBoxMonday.RightToLeft = System.Windows.Forms.RightToLeft.No;
      this.checkBoxMonday.Size = new System.Drawing.Size(41, 17);
      this.checkBoxMonday.TabIndex = 6;
      this.checkBoxMonday.Text = "Mo";
      this.checkBoxMonday.UseVisualStyleBackColor = true;
      this.checkBoxMonday.CheckedChanged += new System.EventHandler(this.checkBoxMonday_CheckedChanged);
      // 
      // groupBox4
      // 
      this.groupBox4.Controls.Add(this.label9);
      this.groupBox4.Controls.Add(this.mpNumericTextBoxStartingAroundDeviation);
      this.groupBox4.Controls.Add(this.dateTimePickerStartingBetweenTo);
      this.groupBox4.Controls.Add(this.label6);
      this.groupBox4.Controls.Add(this.dateTimePickerStartingBetweenFrom);
      this.groupBox4.Controls.Add(this.label2);
      this.groupBox4.Controls.Add(this.dateTimePickerStartingAround);
      this.groupBox4.Controls.Add(this.label25);
      this.groupBox4.Controls.Add(this.dateTimePickerOnDate);
      this.groupBox4.Controls.Add(this.mpLabel2);
      this.groupBox4.Controls.Add(this.checkBoxMonday);
      this.groupBox4.Controls.Add(this.checkBoxSunday);
      this.groupBox4.Controls.Add(this.checkBoxTuesday);
      this.groupBox4.Controls.Add(this.checkBoxSaturday);
      this.groupBox4.Controls.Add(this.checkBoxWednesday);
      this.groupBox4.Controls.Add(this.checkBoxFriday);
      this.groupBox4.Controls.Add(this.checkBoxThursday);
      this.groupBox4.Location = new System.Drawing.Point(578, 72);
      this.groupBox4.Name = "groupBox4";
      this.groupBox4.Size = new System.Drawing.Size(454, 115);
      this.groupBox4.TabIndex = 10;
      this.groupBox4.TabStop = false;
      this.groupBox4.Text = "When";
      // 
      // label9
      // 
      this.label9.AutoSize = true;
      this.label9.Location = new System.Drawing.Point(209, 65);
      this.label9.Name = "label9";
      this.label9.Size = new System.Drawing.Size(78, 13);
      this.label9.TabIndex = 30;
      this.label9.Text = "Deviation (-/+):";
      // 
      // mpNumericTextBoxStartingAroundDeviation
      // 
      this.mpNumericTextBoxStartingAroundDeviation.Location = new System.Drawing.Point(293, 62);
      this.mpNumericTextBoxStartingAroundDeviation.Name = "mpNumericTextBoxStartingAroundDeviation";
      this.mpNumericTextBoxStartingAroundDeviation.Size = new System.Drawing.Size(52, 20);
      this.mpNumericTextBoxStartingAroundDeviation.TabIndex = 29;
      this.mpNumericTextBoxStartingAroundDeviation.Text = "0";
      this.mpNumericTextBoxStartingAroundDeviation.Value = 0;
      // 
      // dateTimePickerStartingBetweenTo
      // 
      this.dateTimePickerStartingBetweenTo.Checked = false;
      this.dateTimePickerStartingBetweenTo.CustomFormat = "HH:mm";
      this.dateTimePickerStartingBetweenTo.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
      this.dateTimePickerStartingBetweenTo.Location = new System.Drawing.Point(211, 88);
      this.dateTimePickerStartingBetweenTo.Name = "dateTimePickerStartingBetweenTo";
      this.dateTimePickerStartingBetweenTo.ShowCheckBox = true;
      this.dateTimePickerStartingBetweenTo.ShowUpDown = true;
      this.dateTimePickerStartingBetweenTo.Size = new System.Drawing.Size(83, 20);
      this.dateTimePickerStartingBetweenTo.TabIndex = 23;
      // 
      // label6
      // 
      this.label6.AutoSize = true;
      this.label6.Location = new System.Drawing.Point(6, 88);
      this.label6.Name = "label6";
      this.label6.Size = new System.Drawing.Size(90, 13);
      this.label6.TabIndex = 22;
      this.label6.Text = "Starting between:";
      // 
      // dateTimePickerStartingBetweenFrom
      // 
      this.dateTimePickerStartingBetweenFrom.Checked = false;
      this.dateTimePickerStartingBetweenFrom.CustomFormat = "HH:mm";
      this.dateTimePickerStartingBetweenFrom.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
      this.dateTimePickerStartingBetweenFrom.Location = new System.Drawing.Point(117, 88);
      this.dateTimePickerStartingBetweenFrom.Name = "dateTimePickerStartingBetweenFrom";
      this.dateTimePickerStartingBetweenFrom.ShowCheckBox = true;
      this.dateTimePickerStartingBetweenFrom.ShowUpDown = true;
      this.dateTimePickerStartingBetweenFrom.Size = new System.Drawing.Size(83, 20);
      this.dateTimePickerStartingBetweenFrom.TabIndex = 21;
      // 
      // label2
      // 
      this.label2.AutoSize = true;
      this.label2.Location = new System.Drawing.Point(6, 62);
      this.label2.Name = "label2";
      this.label2.Size = new System.Drawing.Size(82, 13);
      this.label2.TabIndex = 20;
      this.label2.Text = "Starting around:";
      // 
      // dateTimePickerStartingAround
      // 
      this.dateTimePickerStartingAround.CustomFormat = "HH:mm";
      this.dateTimePickerStartingAround.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
      this.dateTimePickerStartingAround.Location = new System.Drawing.Point(117, 62);
      this.dateTimePickerStartingAround.Name = "dateTimePickerStartingAround";
      this.dateTimePickerStartingAround.ShowCheckBox = true;
      this.dateTimePickerStartingAround.ShowUpDown = true;
      this.dateTimePickerStartingAround.Size = new System.Drawing.Size(83, 20);
      this.dateTimePickerStartingAround.TabIndex = 19;
      // 
      // label25
      // 
      this.label25.AutoSize = true;
      this.label25.Location = new System.Drawing.Point(6, 19);
      this.label25.Name = "label25";
      this.label25.Size = new System.Drawing.Size(48, 13);
      this.label25.TabIndex = 18;
      this.label25.Text = "On date:";
      // 
      // dateTimePickerOnDate
      // 
      this.dateTimePickerOnDate.CustomFormat = "dd/MM/yyyy";
      this.dateTimePickerOnDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
      this.dateTimePickerOnDate.Location = new System.Drawing.Point(70, 15);
      this.dateTimePickerOnDate.Name = "dateTimePickerOnDate";
      this.dateTimePickerOnDate.ShowCheckBox = true;
      this.dateTimePickerOnDate.Size = new System.Drawing.Size(157, 20);
      this.dateTimePickerOnDate.TabIndex = 17;
      this.dateTimePickerOnDate.ValueChanged += new System.EventHandler(this.dateTimePickerOnDate_ValueChanged);
      // 
      // mpLabel2
      // 
      this.mpLabel2.AutoSize = true;
      this.mpLabel2.Location = new System.Drawing.Point(6, 40);
      this.mpLabel2.Name = "mpLabel2";
      this.mpLabel2.Size = new System.Drawing.Size(49, 13);
      this.mpLabel2.TabIndex = 9;
      this.mpLabel2.Text = "On days:";
      // 
      // checkBoxSunday
      // 
      this.checkBoxSunday.AutoSize = true;
      this.checkBoxSunday.Location = new System.Drawing.Point(342, 39);
      this.checkBoxSunday.Name = "checkBoxSunday";
      this.checkBoxSunday.RightToLeft = System.Windows.Forms.RightToLeft.No;
      this.checkBoxSunday.Size = new System.Drawing.Size(39, 17);
      this.checkBoxSunday.TabIndex = 16;
      this.checkBoxSunday.Text = "Su";
      this.checkBoxSunday.UseVisualStyleBackColor = true;
      this.checkBoxSunday.CheckedChanged += new System.EventHandler(this.checkBoxSunday_CheckedChanged);
      // 
      // checkBoxTuesday
      // 
      this.checkBoxTuesday.AutoSize = true;
      this.checkBoxTuesday.Location = new System.Drawing.Point(117, 39);
      this.checkBoxTuesday.Name = "checkBoxTuesday";
      this.checkBoxTuesday.RightToLeft = System.Windows.Forms.RightToLeft.No;
      this.checkBoxTuesday.Size = new System.Drawing.Size(39, 17);
      this.checkBoxTuesday.TabIndex = 11;
      this.checkBoxTuesday.Text = "Tu";
      this.checkBoxTuesday.UseVisualStyleBackColor = true;
      this.checkBoxTuesday.CheckedChanged += new System.EventHandler(this.checkBoxTuesday_CheckedChanged);
      // 
      // checkBoxSaturday
      // 
      this.checkBoxSaturday.AutoSize = true;
      this.checkBoxSaturday.Location = new System.Drawing.Point(297, 39);
      this.checkBoxSaturday.Name = "checkBoxSaturday";
      this.checkBoxSaturday.RightToLeft = System.Windows.Forms.RightToLeft.No;
      this.checkBoxSaturday.Size = new System.Drawing.Size(39, 17);
      this.checkBoxSaturday.TabIndex = 15;
      this.checkBoxSaturday.Text = "Sa";
      this.checkBoxSaturday.UseVisualStyleBackColor = true;
      this.checkBoxSaturday.CheckedChanged += new System.EventHandler(this.checkBoxSaturday_CheckedChanged);
      // 
      // checkBoxWednesday
      // 
      this.checkBoxWednesday.AutoSize = true;
      this.checkBoxWednesday.Location = new System.Drawing.Point(162, 39);
      this.checkBoxWednesday.Name = "checkBoxWednesday";
      this.checkBoxWednesday.RightToLeft = System.Windows.Forms.RightToLeft.No;
      this.checkBoxWednesday.Size = new System.Drawing.Size(43, 17);
      this.checkBoxWednesday.TabIndex = 12;
      this.checkBoxWednesday.Text = "We";
      this.checkBoxWednesday.UseVisualStyleBackColor = true;
      this.checkBoxWednesday.CheckedChanged += new System.EventHandler(this.checkBoxWednesday_CheckedChanged);
      // 
      // checkBoxFriday
      // 
      this.checkBoxFriday.AutoSize = true;
      this.checkBoxFriday.Location = new System.Drawing.Point(256, 39);
      this.checkBoxFriday.Name = "checkBoxFriday";
      this.checkBoxFriday.RightToLeft = System.Windows.Forms.RightToLeft.No;
      this.checkBoxFriday.Size = new System.Drawing.Size(35, 17);
      this.checkBoxFriday.TabIndex = 14;
      this.checkBoxFriday.Text = "Fr";
      this.checkBoxFriday.UseVisualStyleBackColor = true;
      this.checkBoxFriday.CheckedChanged += new System.EventHandler(this.checkBoxFriday_CheckedChanged);
      // 
      // checkBoxThursday
      // 
      this.checkBoxThursday.AutoSize = true;
      this.checkBoxThursday.Location = new System.Drawing.Point(211, 39);
      this.checkBoxThursday.Name = "checkBoxThursday";
      this.checkBoxThursday.RightToLeft = System.Windows.Forms.RightToLeft.No;
      this.checkBoxThursday.Size = new System.Drawing.Size(39, 17);
      this.checkBoxThursday.TabIndex = 13;
      this.checkBoxThursday.Text = "Th";
      this.checkBoxThursday.UseVisualStyleBackColor = true;
      this.checkBoxThursday.CheckedChanged += new System.EventHandler(this.checkBoxThursday_CheckedChanged);
      // 
      // groupBox5
      // 
      this.groupBox5.Controls.Add(this.mpButtonCreditAdd);
      this.groupBox5.Controls.Add(this.mpButtonCreditRemove);
      this.groupBox5.Controls.Add(this.mpTextBoxPerson);
      this.groupBox5.Controls.Add(this.mpComboBoxRoles);
      this.groupBox5.Controls.Add(this.listBoxCredits);
      this.groupBox5.Location = new System.Drawing.Point(735, 193);
      this.groupBox5.Name = "groupBox5";
      this.groupBox5.Size = new System.Drawing.Size(412, 133);
      this.groupBox5.TabIndex = 11;
      this.groupBox5.TabStop = false;
      this.groupBox5.Text = "Credits";
      // 
      // mpButtonCreditAdd
      // 
      this.mpButtonCreditAdd.Location = new System.Drawing.Point(138, 46);
      this.mpButtonCreditAdd.Name = "mpButtonCreditAdd";
      this.mpButtonCreditAdd.Size = new System.Drawing.Size(75, 23);
      this.mpButtonCreditAdd.TabIndex = 11;
      this.mpButtonCreditAdd.Text = "Add";
      this.mpButtonCreditAdd.UseVisualStyleBackColor = true;
      this.mpButtonCreditAdd.Click += new System.EventHandler(this.mpButtonCreditAdd_Click);
      // 
      // mpButtonCreditRemove
      // 
      this.mpButtonCreditRemove.Location = new System.Drawing.Point(138, 100);
      this.mpButtonCreditRemove.Name = "mpButtonCreditRemove";
      this.mpButtonCreditRemove.Size = new System.Drawing.Size(75, 23);
      this.mpButtonCreditRemove.TabIndex = 10;
      this.mpButtonCreditRemove.Text = "Remove";
      this.mpButtonCreditRemove.UseVisualStyleBackColor = true;
      this.mpButtonCreditRemove.Click += new System.EventHandler(this.mpButtonCreditRemove_Click);
      // 
      // mpTextBoxPerson
      // 
      this.mpTextBoxPerson.Location = new System.Drawing.Point(247, 19);
      this.mpTextBoxPerson.Name = "mpTextBoxPerson";
      this.mpTextBoxPerson.Size = new System.Drawing.Size(159, 20);
      this.mpTextBoxPerson.TabIndex = 9;
      // 
      // mpComboBoxRoles
      // 
      this.mpComboBoxRoles.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.mpComboBoxRoles.FormattingEnabled = true;
      this.mpComboBoxRoles.Location = new System.Drawing.Point(138, 19);
      this.mpComboBoxRoles.Name = "mpComboBoxRoles";
      this.mpComboBoxRoles.Size = new System.Drawing.Size(103, 21);
      this.mpComboBoxRoles.TabIndex = 8;
      // 
      // listBoxCredits
      // 
      this.listBoxCredits.Location = new System.Drawing.Point(6, 19);
      this.listBoxCredits.Name = "listBoxCredits";
      this.listBoxCredits.Size = new System.Drawing.Size(121, 94);
      this.listBoxCredits.TabIndex = 0;
      // 
      // groupBox1
      // 
      this.groupBox1.Controls.Add(this.listBoxPrograms);
      this.groupBox1.Controls.Add(this.mpComboBoxOperators);
      this.groupBox1.Controls.Add(this.mpButtonAddProgramCondition);
      this.groupBox1.Controls.Add(this.mpButtonRemoveProgramSchedule);
      this.groupBox1.Controls.Add(this.mpTextBoxProgramValue);
      this.groupBox1.Controls.Add(this.mpComboBoxProgramFields);
      this.groupBox1.Location = new System.Drawing.Point(12, 72);
      this.groupBox1.Name = "groupBox1";
      this.groupBox1.Size = new System.Drawing.Size(560, 115);
      this.groupBox1.TabIndex = 12;
      this.groupBox1.TabStop = false;
      this.groupBox1.Text = "Program";
      // 
      // listBoxPrograms
      // 
      this.listBoxPrograms.Location = new System.Drawing.Point(9, 15);
      this.listBoxPrograms.Name = "listBoxPrograms";
      this.listBoxPrograms.SelectionMode = System.Windows.Forms.SelectionMode.MultiSimple;
      this.listBoxPrograms.Size = new System.Drawing.Size(221, 82);
      this.listBoxPrograms.TabIndex = 17;
      // 
      // mpComboBoxOperators
      // 
      this.mpComboBoxOperators.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.mpComboBoxOperators.FormattingEnabled = true;
      this.mpComboBoxOperators.Location = new System.Drawing.Point(345, 19);
      this.mpComboBoxOperators.Name = "mpComboBoxOperators";
      this.mpComboBoxOperators.Size = new System.Drawing.Size(80, 21);
      this.mpComboBoxOperators.TabIndex = 12;
      // 
      // mpButtonAddProgramCondition
      // 
      this.mpButtonAddProgramCondition.Location = new System.Drawing.Point(236, 46);
      this.mpButtonAddProgramCondition.Name = "mpButtonAddProgramCondition";
      this.mpButtonAddProgramCondition.Size = new System.Drawing.Size(75, 23);
      this.mpButtonAddProgramCondition.TabIndex = 11;
      this.mpButtonAddProgramCondition.Text = "Add";
      this.mpButtonAddProgramCondition.UseVisualStyleBackColor = true;
      this.mpButtonAddProgramCondition.Click += new System.EventHandler(this.mpButton1_Click);
      // 
      // mpButtonRemoveProgramSchedule
      // 
      this.mpButtonRemoveProgramSchedule.Location = new System.Drawing.Point(236, 75);
      this.mpButtonRemoveProgramSchedule.Name = "mpButtonRemoveProgramSchedule";
      this.mpButtonRemoveProgramSchedule.Size = new System.Drawing.Size(75, 23);
      this.mpButtonRemoveProgramSchedule.TabIndex = 10;
      this.mpButtonRemoveProgramSchedule.Text = "Remove";
      this.mpButtonRemoveProgramSchedule.UseVisualStyleBackColor = true;
      this.mpButtonRemoveProgramSchedule.Click += new System.EventHandler(this.mpButton2_Click);
      // 
      // mpTextBoxProgramValue
      // 
      this.mpTextBoxProgramValue.Location = new System.Drawing.Point(431, 19);
      this.mpTextBoxProgramValue.Name = "mpTextBoxProgramValue";
      this.mpTextBoxProgramValue.Size = new System.Drawing.Size(125, 20);
      this.mpTextBoxProgramValue.TabIndex = 9;
      // 
      // mpComboBoxProgramFields
      // 
      this.mpComboBoxProgramFields.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.mpComboBoxProgramFields.FormattingEnabled = true;
      this.mpComboBoxProgramFields.Location = new System.Drawing.Point(236, 19);
      this.mpComboBoxProgramFields.Name = "mpComboBoxProgramFields";
      this.mpComboBoxProgramFields.Size = new System.Drawing.Size(103, 21);
      this.mpComboBoxProgramFields.TabIndex = 8;
      this.mpComboBoxProgramFields.SelectedIndexChanged += new System.EventHandler(this.mpComboBoxProgramFields_SelectedIndexChanged);
      // 
      // groupBox2
      // 
      this.groupBox2.Controls.Add(this.radioOnAllChannels);
      this.groupBox2.Controls.Add(this.mpButtonAddAllChannelCondition);
      this.groupBox2.Controls.Add(this.radioNotOnChannels);
      this.groupBox2.Controls.Add(this.radioOnChannels);
      this.groupBox2.Controls.Add(this.mpComboBoxChannels);
      this.groupBox2.Controls.Add(this.mpButtonAddChannelCondition);
      this.groupBox2.Controls.Add(this.mpButtonRemoveChannelCondition);
      this.groupBox2.Controls.Add(this.mpComboBoxChannelsGroup);
      this.groupBox2.Controls.Add(this.listBoxChannels);
      this.groupBox2.Location = new System.Drawing.Point(12, 193);
      this.groupBox2.Name = "groupBox2";
      this.groupBox2.Size = new System.Drawing.Size(412, 133);
      this.groupBox2.TabIndex = 13;
      this.groupBox2.TabStop = false;
      this.groupBox2.Text = "Channels";
      // 
      // radioOnAllChannels
      // 
      this.radioOnAllChannels.AutoSize = true;
      this.radioOnAllChannels.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
      this.radioOnAllChannels.Location = new System.Drawing.Point(9, 19);
      this.radioOnAllChannels.Name = "radioOnAllChannels";
      this.radioOnAllChannels.Size = new System.Drawing.Size(97, 17);
      this.radioOnAllChannels.TabIndex = 16;
      this.radioOnAllChannels.Text = "On all channels";
      this.radioOnAllChannels.UseVisualStyleBackColor = true;
      this.radioOnAllChannels.CheckedChanged += new System.EventHandler(this.radioOnAllChannels_CheckedChanged);
      // 
      // mpButtonAddAllChannelCondition
      // 
      this.mpButtonAddAllChannelCondition.Location = new System.Drawing.Point(276, 70);
      this.mpButtonAddAllChannelCondition.Name = "mpButtonAddAllChannelCondition";
      this.mpButtonAddAllChannelCondition.Size = new System.Drawing.Size(75, 23);
      this.mpButtonAddAllChannelCondition.TabIndex = 15;
      this.mpButtonAddAllChannelCondition.Text = "Add All";
      this.mpButtonAddAllChannelCondition.UseVisualStyleBackColor = true;
      // 
      // radioNotOnChannels
      // 
      this.radioNotOnChannels.AutoSize = true;
      this.radioNotOnChannels.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
      this.radioNotOnChannels.Location = new System.Drawing.Point(195, 19);
      this.radioNotOnChannels.Name = "radioNotOnChannels";
      this.radioNotOnChannels.Size = new System.Drawing.Size(102, 17);
      this.radioNotOnChannels.TabIndex = 14;
      this.radioNotOnChannels.Text = "Not on channels";
      this.radioNotOnChannels.UseVisualStyleBackColor = true;
      this.radioNotOnChannels.CheckedChanged += new System.EventHandler(this.radioNotOnChannels_CheckedChanged);
      // 
      // radioOnChannels
      // 
      this.radioOnChannels.AutoSize = true;
      this.radioOnChannels.Checked = true;
      this.radioOnChannels.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
      this.radioOnChannels.Location = new System.Drawing.Point(112, 19);
      this.radioOnChannels.Name = "radioOnChannels";
      this.radioOnChannels.Size = new System.Drawing.Size(68, 17);
      this.radioOnChannels.TabIndex = 13;
      this.radioOnChannels.TabStop = true;
      this.radioOnChannels.Text = "Channels";
      this.radioOnChannels.UseVisualStyleBackColor = true;
      this.radioOnChannels.CheckedChanged += new System.EventHandler(this.radioOnChannels_CheckedChanged);
      // 
      // mpComboBoxChannels
      // 
      this.mpComboBoxChannels.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.mpComboBoxChannels.FormattingEnabled = true;
      this.mpComboBoxChannels.Location = new System.Drawing.Point(304, 43);
      this.mpComboBoxChannels.Name = "mpComboBoxChannels";
      this.mpComboBoxChannels.Size = new System.Drawing.Size(103, 21);
      this.mpComboBoxChannels.TabIndex = 12;
      // 
      // mpButtonAddChannelCondition
      // 
      this.mpButtonAddChannelCondition.Location = new System.Drawing.Point(195, 70);
      this.mpButtonAddChannelCondition.Name = "mpButtonAddChannelCondition";
      this.mpButtonAddChannelCondition.Size = new System.Drawing.Size(75, 23);
      this.mpButtonAddChannelCondition.TabIndex = 11;
      this.mpButtonAddChannelCondition.Text = "Add";
      this.mpButtonAddChannelCondition.UseVisualStyleBackColor = true;
      this.mpButtonAddChannelCondition.Click += new System.EventHandler(this.mpButtonAddChannelCondition_Click);
      // 
      // mpButtonRemoveChannelCondition
      // 
      this.mpButtonRemoveChannelCondition.Location = new System.Drawing.Point(195, 100);
      this.mpButtonRemoveChannelCondition.Name = "mpButtonRemoveChannelCondition";
      this.mpButtonRemoveChannelCondition.Size = new System.Drawing.Size(75, 23);
      this.mpButtonRemoveChannelCondition.TabIndex = 10;
      this.mpButtonRemoveChannelCondition.Text = "Remove";
      this.mpButtonRemoveChannelCondition.UseVisualStyleBackColor = true;
      this.mpButtonRemoveChannelCondition.Click += new System.EventHandler(this.mpButtonRemoveChannelCondition_Click);
      // 
      // mpComboBoxChannelsGroup
      // 
      this.mpComboBoxChannelsGroup.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.mpComboBoxChannelsGroup.FormattingEnabled = true;
      this.mpComboBoxChannelsGroup.Location = new System.Drawing.Point(195, 43);
      this.mpComboBoxChannelsGroup.Name = "mpComboBoxChannelsGroup";
      this.mpComboBoxChannelsGroup.Size = new System.Drawing.Size(103, 21);
      this.mpComboBoxChannelsGroup.TabIndex = 8;
      this.mpComboBoxChannelsGroup.SelectedIndexChanged += new System.EventHandler(this.mpComboBoxChannelsGroup_SelectedIndexChanged);
      // 
      // listBoxChannels
      // 
      this.listBoxChannels.Location = new System.Drawing.Point(6, 43);
      this.listBoxChannels.Name = "listBoxChannels";
      this.listBoxChannels.SelectionMode = System.Windows.Forms.SelectionMode.MultiSimple;
      this.listBoxChannels.Size = new System.Drawing.Size(183, 82);
      this.listBoxChannels.TabIndex = 0;
      // 
      // groupBox3
      // 
      this.groupBox3.Controls.Add(this.mpRadioButtonInAllCategories);
      this.groupBox3.Controls.Add(this.mpButtonAddAllCategoryCondition);
      this.groupBox3.Controls.Add(this.mpRadioButtonNotInCategory);
      this.groupBox3.Controls.Add(this.mpRadioButtonInCategory);
      this.groupBox3.Controls.Add(this.mpButtonAddCategoryCondition);
      this.groupBox3.Controls.Add(this.mpButtonRemoveCategoryCondition);
      this.groupBox3.Controls.Add(this.mpComboBoxCategories);
      this.groupBox3.Controls.Add(this.listBoxCategories);
      this.groupBox3.Location = new System.Drawing.Point(431, 193);
      this.groupBox3.Name = "groupBox3";
      this.groupBox3.Size = new System.Drawing.Size(298, 133);
      this.groupBox3.TabIndex = 14;
      this.groupBox3.TabStop = false;
      this.groupBox3.Text = "Categories";
      // 
      // mpRadioButtonInAllCategories
      // 
      this.mpRadioButtonInAllCategories.AutoSize = true;
      this.mpRadioButtonInAllCategories.Checked = true;
      this.mpRadioButtonInAllCategories.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
      this.mpRadioButtonInAllCategories.Location = new System.Drawing.Point(6, 20);
      this.mpRadioButtonInAllCategories.Name = "mpRadioButtonInAllCategories";
      this.mpRadioButtonInAllCategories.Size = new System.Drawing.Size(98, 17);
      this.mpRadioButtonInAllCategories.TabIndex = 16;
      this.mpRadioButtonInAllCategories.TabStop = true;
      this.mpRadioButtonInAllCategories.Text = "In all categories";
      this.mpRadioButtonInAllCategories.UseVisualStyleBackColor = true;
      this.mpRadioButtonInAllCategories.CheckedChanged += new System.EventHandler(this.mpRadioButtonInAllCategories_CheckedChanged);
      // 
      // mpButtonAddAllCategoryCondition
      // 
      this.mpButtonAddAllCategoryCondition.Location = new System.Drawing.Point(219, 70);
      this.mpButtonAddAllCategoryCondition.Name = "mpButtonAddAllCategoryCondition";
      this.mpButtonAddAllCategoryCondition.Size = new System.Drawing.Size(75, 23);
      this.mpButtonAddAllCategoryCondition.TabIndex = 15;
      this.mpButtonAddAllCategoryCondition.Text = "Add All";
      this.mpButtonAddAllCategoryCondition.UseVisualStyleBackColor = true;
      // 
      // mpRadioButtonNotInCategory
      // 
      this.mpRadioButtonNotInCategory.AutoSize = true;
      this.mpRadioButtonNotInCategory.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
      this.mpRadioButtonNotInCategory.Location = new System.Drawing.Point(187, 20);
      this.mpRadioButtonNotInCategory.Name = "mpRadioButtonNotInCategory";
      this.mpRadioButtonNotInCategory.Size = new System.Drawing.Size(104, 17);
      this.mpRadioButtonNotInCategory.TabIndex = 14;
      this.mpRadioButtonNotInCategory.Text = "Not in categories";
      this.mpRadioButtonNotInCategory.UseVisualStyleBackColor = true;
      this.mpRadioButtonNotInCategory.CheckedChanged += new System.EventHandler(this.mpRadioButtonNotInCategory_CheckedChanged);
      // 
      // mpRadioButtonInCategory
      // 
      this.mpRadioButtonInCategory.AutoSize = true;
      this.mpRadioButtonInCategory.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
      this.mpRadioButtonInCategory.Location = new System.Drawing.Point(113, 20);
      this.mpRadioButtonInCategory.Name = "mpRadioButtonInCategory";
      this.mpRadioButtonInCategory.Size = new System.Drawing.Size(74, 17);
      this.mpRadioButtonInCategory.TabIndex = 13;
      this.mpRadioButtonInCategory.Text = "Categories";
      this.mpRadioButtonInCategory.UseVisualStyleBackColor = true;
      this.mpRadioButtonInCategory.CheckedChanged += new System.EventHandler(this.mpRadioButtonInCategory_CheckedChanged);
      // 
      // mpButtonAddCategoryCondition
      // 
      this.mpButtonAddCategoryCondition.Location = new System.Drawing.Point(138, 70);
      this.mpButtonAddCategoryCondition.Name = "mpButtonAddCategoryCondition";
      this.mpButtonAddCategoryCondition.Size = new System.Drawing.Size(75, 23);
      this.mpButtonAddCategoryCondition.TabIndex = 11;
      this.mpButtonAddCategoryCondition.Text = "Add";
      this.mpButtonAddCategoryCondition.UseVisualStyleBackColor = true;
      this.mpButtonAddCategoryCondition.Click += new System.EventHandler(this.mpButtonAddCategoryCondition_Click);
      // 
      // mpButtonRemoveCategoryCondition
      // 
      this.mpButtonRemoveCategoryCondition.Location = new System.Drawing.Point(138, 100);
      this.mpButtonRemoveCategoryCondition.Name = "mpButtonRemoveCategoryCondition";
      this.mpButtonRemoveCategoryCondition.Size = new System.Drawing.Size(75, 23);
      this.mpButtonRemoveCategoryCondition.TabIndex = 10;
      this.mpButtonRemoveCategoryCondition.Text = "Remove";
      this.mpButtonRemoveCategoryCondition.UseVisualStyleBackColor = true;
      this.mpButtonRemoveCategoryCondition.Click += new System.EventHandler(this.mpButtonRemoveCategoryCondition_Click);
      // 
      // mpComboBoxCategories
      // 
      this.mpComboBoxCategories.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.mpComboBoxCategories.FormattingEnabled = true;
      this.mpComboBoxCategories.Location = new System.Drawing.Point(138, 43);
      this.mpComboBoxCategories.Name = "mpComboBoxCategories";
      this.mpComboBoxCategories.Size = new System.Drawing.Size(103, 21);
      this.mpComboBoxCategories.TabIndex = 8;
      // 
      // listBoxCategories
      // 
      this.listBoxCategories.Location = new System.Drawing.Point(6, 43);
      this.listBoxCategories.Name = "listBoxCategories";
      this.listBoxCategories.Size = new System.Drawing.Size(121, 79);
      this.listBoxCategories.TabIndex = 0;
      // 
      // groupBox6
      // 
      this.groupBox6.Controls.Add(this.label1);
      this.groupBox6.Controls.Add(this.checkBoxSkipRepeats);
      this.groupBox6.Controls.Add(this.checkBoxNewTitles);
      this.groupBox6.Controls.Add(this.checkBoxOnlyNewEpisodes);
      this.groupBox6.Location = new System.Drawing.Point(1038, 72);
      this.groupBox6.Name = "groupBox6";
      this.groupBox6.Size = new System.Drawing.Size(109, 115);
      this.groupBox6.TabIndex = 16;
      this.groupBox6.TabStop = false;
      this.groupBox6.Text = "Misc.";
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Location = new System.Drawing.Point(6, 16);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(68, 13);
      this.label1.TabIndex = 23;
      this.label1.Text = "Only include:";
      // 
      // checkBoxSkipRepeats
      // 
      this.checkBoxSkipRepeats.AutoSize = true;
      this.checkBoxSkipRepeats.Location = new System.Drawing.Point(9, 59);
      this.checkBoxSkipRepeats.Name = "checkBoxSkipRepeats";
      this.checkBoxSkipRepeats.RightToLeft = System.Windows.Forms.RightToLeft.No;
      this.checkBoxSkipRepeats.Size = new System.Drawing.Size(85, 17);
      this.checkBoxSkipRepeats.TabIndex = 9;
      this.checkBoxSkipRepeats.Text = "Skip repeats";
      this.checkBoxSkipRepeats.UseVisualStyleBackColor = true;
      // 
      // checkBoxNewTitles
      // 
      this.checkBoxNewTitles.AutoSize = true;
      this.checkBoxNewTitles.Location = new System.Drawing.Point(9, 84);
      this.checkBoxNewTitles.Name = "checkBoxNewTitles";
      this.checkBoxNewTitles.RightToLeft = System.Windows.Forms.RightToLeft.No;
      this.checkBoxNewTitles.Size = new System.Drawing.Size(72, 17);
      this.checkBoxNewTitles.TabIndex = 8;
      this.checkBoxNewTitles.Text = "New titles";
      this.checkBoxNewTitles.UseVisualStyleBackColor = true;
      // 
      // checkBoxOnlyNewEpisodes
      // 
      this.checkBoxOnlyNewEpisodes.AutoSize = true;
      this.checkBoxOnlyNewEpisodes.Location = new System.Drawing.Point(9, 36);
      this.checkBoxOnlyNewEpisodes.Name = "checkBoxOnlyNewEpisodes";
      this.checkBoxOnlyNewEpisodes.RightToLeft = System.Windows.Forms.RightToLeft.No;
      this.checkBoxOnlyNewEpisodes.Size = new System.Drawing.Size(93, 17);
      this.checkBoxOnlyNewEpisodes.TabIndex = 7;
      this.checkBoxOnlyNewEpisodes.Text = "New episodes";
      this.checkBoxOnlyNewEpisodes.UseVisualStyleBackColor = true;
      // 
      // groupBox7
      // 
      this.groupBox7.Controls.Add(this.mpNumericTextBoxPostRec);
      this.groupBox7.Controls.Add(this.mpNumericTextBoxPreRec);
      this.groupBox7.Controls.Add(this.label8);
      this.groupBox7.Controls.Add(this.mpNumericTextBoxPriority);
      this.groupBox7.Controls.Add(this.label7);
      this.groupBox7.Controls.Add(this.mpComboBoxKeepMethods);
      this.groupBox7.Controls.Add(this.mpTextBoxScheduleName);
      this.groupBox7.Controls.Add(this.label5);
      this.groupBox7.Controls.Add(this.label4);
      this.groupBox7.Controls.Add(this.label3);
      this.groupBox7.Location = new System.Drawing.Point(12, 12);
      this.groupBox7.Name = "groupBox7";
      this.groupBox7.Size = new System.Drawing.Size(1135, 54);
      this.groupBox7.TabIndex = 17;
      this.groupBox7.TabStop = false;
      this.groupBox7.Text = "Schedule";
      // 
      // mpNumericTextBoxPostRec
      // 
      this.mpNumericTextBoxPostRec.Location = new System.Drawing.Point(911, 19);
      this.mpNumericTextBoxPostRec.Name = "mpNumericTextBoxPostRec";
      this.mpNumericTextBoxPostRec.Size = new System.Drawing.Size(44, 20);
      this.mpNumericTextBoxPostRec.TabIndex = 30;
      this.mpNumericTextBoxPostRec.Text = "0";
      this.mpNumericTextBoxPostRec.Value = 0;
      // 
      // mpNumericTextBoxPreRec
      // 
      this.mpNumericTextBoxPreRec.Location = new System.Drawing.Point(762, 19);
      this.mpNumericTextBoxPreRec.Name = "mpNumericTextBoxPreRec";
      this.mpNumericTextBoxPreRec.Size = new System.Drawing.Size(52, 20);
      this.mpNumericTextBoxPreRec.TabIndex = 29;
      this.mpNumericTextBoxPreRec.Text = "0";
      this.mpNumericTextBoxPreRec.Value = 0;
      // 
      // label8
      // 
      this.label8.AutoSize = true;
      this.label8.Location = new System.Drawing.Point(582, 22);
      this.label8.Name = "label8";
      this.label8.Size = new System.Drawing.Size(41, 13);
      this.label8.TabIndex = 28;
      this.label8.Text = "Priority:";
      // 
      // mpNumericTextBoxPriority
      // 
      this.mpNumericTextBoxPriority.Location = new System.Drawing.Point(629, 19);
      this.mpNumericTextBoxPriority.Name = "mpNumericTextBoxPriority";
      this.mpNumericTextBoxPriority.Size = new System.Drawing.Size(52, 20);
      this.mpNumericTextBoxPriority.TabIndex = 27;
      this.mpNumericTextBoxPriority.Text = "0";
      this.mpNumericTextBoxPriority.Value = 0;
      // 
      // label7
      // 
      this.label7.AutoSize = true;
      this.label7.Location = new System.Drawing.Point(985, 22);
      this.label7.Name = "label7";
      this.label7.Size = new System.Drawing.Size(35, 13);
      this.label7.TabIndex = 26;
      this.label7.Text = "Keep:";
      // 
      // mpComboBoxKeepMethods
      // 
      this.mpComboBoxKeepMethods.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.mpComboBoxKeepMethods.FormattingEnabled = true;
      this.mpComboBoxKeepMethods.Location = new System.Drawing.Point(1026, 19);
      this.mpComboBoxKeepMethods.Name = "mpComboBoxKeepMethods";
      this.mpComboBoxKeepMethods.Size = new System.Drawing.Size(103, 21);
      this.mpComboBoxKeepMethods.TabIndex = 25;
      // 
      // mpTextBoxScheduleName
      // 
      this.mpTextBoxScheduleName.Location = new System.Drawing.Point(66, 19);
      this.mpTextBoxScheduleName.Name = "mpTextBoxScheduleName";
      this.mpTextBoxScheduleName.Size = new System.Drawing.Size(488, 20);
      this.mpTextBoxScheduleName.TabIndex = 24;
      // 
      // label5
      // 
      this.label5.AutoSize = true;
      this.label5.Location = new System.Drawing.Point(6, 21);
      this.label5.Name = "label5";
      this.label5.Size = new System.Drawing.Size(38, 13);
      this.label5.TabIndex = 23;
      this.label5.Text = "Name:";
      // 
      // label4
      // 
      this.label4.AutoSize = true;
      this.label4.Location = new System.Drawing.Point(841, 22);
      this.label4.Name = "label4";
      this.label4.Size = new System.Drawing.Size(64, 13);
      this.label4.TabIndex = 22;
      this.label4.Text = "Post-record:";
      // 
      // label3
      // 
      this.label3.AutoSize = true;
      this.label3.Location = new System.Drawing.Point(697, 22);
      this.label3.Name = "label3";
      this.label3.Size = new System.Drawing.Size(59, 13);
      this.label3.TabIndex = 21;
      this.label3.Text = "Pre-record:";
      // 
      // groupBox8
      // 
      this.groupBox8.Controls.Add(this.listView2);
      this.groupBox8.Location = new System.Drawing.Point(12, 332);
      this.groupBox8.Name = "groupBox8";
      this.groupBox8.Size = new System.Drawing.Size(1135, 113);
      this.groupBox8.TabIndex = 18;
      this.groupBox8.TabStop = false;
      this.groupBox8.Text = "Programs";
      // 
      // listView2
      // 
      this.listView2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                  | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.listView2.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader6,
            this.columnHeader7,
            this.columnHeader8,
            this.columnHeader9,
            this.columnHeader10,
            this.columnHeader11,
            this.columnHeader12,
            this.columnHeader13,
            this.columnHeader14,
            this.columnHeader15,
            this.columnHeader16,
            this.columnHeader17,
            this.columnHeader18,
            this.columnHeader19});
      this.listView2.FullRowSelect = true;
      this.listView2.Location = new System.Drawing.Point(6, 19);
      this.listView2.Name = "listView2";
      this.listView2.Size = new System.Drawing.Size(1123, 88);
      this.listView2.TabIndex = 19;
      this.listView2.UseCompatibleStateImageBehavior = false;
      this.listView2.View = System.Windows.Forms.View.Details;
      // 
      // columnHeader6
      // 
      this.columnHeader6.Text = "Title";
      // 
      // columnHeader7
      // 
      this.columnHeader7.Text = "Start";
      // 
      // columnHeader8
      // 
      this.columnHeader8.Text = "End";
      // 
      // columnHeader9
      // 
      this.columnHeader9.Text = "Description";
      // 
      // columnHeader10
      // 
      this.columnHeader10.Text = "Series#";
      // 
      // columnHeader11
      // 
      this.columnHeader11.Text = "Episode#";
      // 
      // columnHeader12
      // 
      this.columnHeader12.Text = "Genre";
      // 
      // columnHeader13
      // 
      this.columnHeader13.Text = "Orig.Air Date";
      // 
      // columnHeader14
      // 
      this.columnHeader14.Text = "Classification";
      // 
      // columnHeader15
      // 
      this.columnHeader15.Text = "StarRating";
      // 
      // columnHeader16
      // 
      this.columnHeader16.Text = "ParentalRating";
      // 
      // columnHeader17
      // 
      this.columnHeader17.Text = "EpisodeName";
      // 
      // columnHeader18
      // 
      this.columnHeader18.Text = "EpisodePart";
      // 
      // columnHeader19
      // 
      this.columnHeader19.Text = "State";
      // 
      // mpButtonSaveTemplate
      // 
      this.mpButtonSaveTemplate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.mpButtonSaveTemplate.Location = new System.Drawing.Point(848, 456);
      this.mpButtonSaveTemplate.Name = "mpButtonSaveTemplate";
      this.mpButtonSaveTemplate.Size = new System.Drawing.Size(114, 23);
      this.mpButtonSaveTemplate.TabIndex = 19;
      this.mpButtonSaveTemplate.Text = "Save as template";
      this.mpButtonSaveTemplate.UseVisualStyleBackColor = true;
      // 
      // mpButtonApplyTemplate
      // 
      this.mpButtonApplyTemplate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.mpButtonApplyTemplate.Location = new System.Drawing.Point(728, 456);
      this.mpButtonApplyTemplate.Name = "mpButtonApplyTemplate";
      this.mpButtonApplyTemplate.Size = new System.Drawing.Size(114, 23);
      this.mpButtonApplyTemplate.TabIndex = 20;
      this.mpButtonApplyTemplate.Text = "Apply template";
      this.mpButtonApplyTemplate.UseVisualStyleBackColor = true;
      // 
      // mpComboBoxTemplates
      // 
      this.mpComboBoxTemplates.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.mpComboBoxTemplates.FormattingEnabled = true;
      this.mpComboBoxTemplates.Location = new System.Drawing.Point(619, 456);
      this.mpComboBoxTemplates.Name = "mpComboBoxTemplates";
      this.mpComboBoxTemplates.Size = new System.Drawing.Size(103, 21);
      this.mpComboBoxTemplates.TabIndex = 21;
      // 
      // FormEditSchedule
      // 
      this.AcceptButton = this.mpButtonSave;
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.CancelButton = this.mpButtonCancel;
      this.ClientSize = new System.Drawing.Size(1159, 492);
      this.Controls.Add(this.mpComboBoxTemplates);
      this.Controls.Add(this.mpButtonApplyTemplate);
      this.Controls.Add(this.mpButtonSaveTemplate);
      this.Controls.Add(this.groupBox8);
      this.Controls.Add(this.groupBox7);
      this.Controls.Add(this.groupBox6);
      this.Controls.Add(this.groupBox3);
      this.Controls.Add(this.groupBox2);
      this.Controls.Add(this.groupBox1);
      this.Controls.Add(this.groupBox5);
      this.Controls.Add(this.groupBox4);
      this.Controls.Add(this.mpButtonCancel);
      this.Controls.Add(this.mpButtonSave);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
      this.Name = "FormEditSchedule";
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
      this.Text = "Add/edit schedule";
      this.Load += new System.EventHandler(this.FormEditSchedule_Load);
      this.groupBox4.ResumeLayout(false);
      this.groupBox4.PerformLayout();
      this.groupBox5.ResumeLayout(false);
      this.groupBox5.PerformLayout();
      this.groupBox1.ResumeLayout(false);
      this.groupBox1.PerformLayout();
      this.groupBox2.ResumeLayout(false);
      this.groupBox2.PerformLayout();
      this.groupBox3.ResumeLayout(false);
      this.groupBox3.PerformLayout();
      this.groupBox6.ResumeLayout(false);
      this.groupBox6.PerformLayout();
      this.groupBox7.ResumeLayout(false);
      this.groupBox7.PerformLayout();
      this.groupBox8.ResumeLayout(false);
      this.ResumeLayout(false);

    }
    

    #endregion

    private MPButton mpButtonSave;
    private MPButton mpButtonCancel;
    private System.Windows.Forms.CheckBox checkBoxMonday;
    private System.Windows.Forms.GroupBox groupBox4;
    private MPLabel mpLabel2;
    private System.Windows.Forms.CheckBox checkBoxSunday;
    private System.Windows.Forms.CheckBox checkBoxTuesday;
    private System.Windows.Forms.CheckBox checkBoxSaturday;
    private System.Windows.Forms.CheckBox checkBoxWednesday;
    private System.Windows.Forms.CheckBox checkBoxFriday;
    private System.Windows.Forms.CheckBox checkBoxThursday;
    private System.Windows.Forms.Label label25;
    private System.Windows.Forms.DateTimePicker dateTimePickerOnDate;
    private System.Windows.Forms.DateTimePicker dateTimePickerStartingBetweenTo;
    private System.Windows.Forms.Label label6;
    private System.Windows.Forms.DateTimePicker dateTimePickerStartingBetweenFrom;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.DateTimePicker dateTimePickerStartingAround;
    private System.Windows.Forms.GroupBox groupBox5;
    private MPButton mpButtonCreditAdd;
    private MPButton mpButtonCreditRemove;
    private MPTextBox mpTextBoxPerson;
    private MPComboBox mpComboBoxRoles;
    private System.Windows.Forms.CheckedListBox listBoxCredits;
    private System.Windows.Forms.GroupBox groupBox1;
    private MPComboBox mpComboBoxOperators;
    private MPButton mpButtonAddProgramCondition;
    private MPButton mpButtonRemoveProgramSchedule;
    private MPTextBox mpTextBoxProgramValue;
    private MPComboBox mpComboBoxProgramFields;
    private System.Windows.Forms.GroupBox groupBox2;
    private MPComboBox mpComboBoxChannels;
    private MPButton mpButtonAddChannelCondition;
    private MPButton mpButtonRemoveChannelCondition;
    private MPComboBox mpComboBoxChannelsGroup;
    private System.Windows.Forms.ListBox listBoxChannels;
    private MPRadioButton radioNotOnChannels;
    private MPRadioButton radioOnChannels;
    private MPButton mpButtonAddAllChannelCondition;
    private System.Windows.Forms.GroupBox groupBox3;
    private MPButton mpButtonAddAllCategoryCondition;
    private MPRadioButton mpRadioButtonNotInCategory;
    private MPRadioButton mpRadioButtonInCategory;
    private MPButton mpButtonAddCategoryCondition;
    private MPButton mpButtonRemoveCategoryCondition;
    private MPComboBox mpComboBoxCategories;
    private System.Windows.Forms.CheckedListBox listBoxCategories;
    private System.Windows.Forms.GroupBox groupBox6;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.CheckBox checkBoxSkipRepeats;
    private System.Windows.Forms.CheckBox checkBoxNewTitles;
    private System.Windows.Forms.CheckBox checkBoxOnlyNewEpisodes;
    private System.Windows.Forms.GroupBox groupBox7;
    private System.Windows.Forms.Label label8;
    private MPNumericTextBox mpNumericTextBoxPriority;
    private System.Windows.Forms.Label label7;
    private MPComboBox mpComboBoxKeepMethods;
    private MPTextBox mpTextBoxScheduleName;
    private System.Windows.Forms.Label label5;
    private System.Windows.Forms.Label label4;
    private System.Windows.Forms.Label label3;
    private System.Windows.Forms.GroupBox groupBox8;
    private System.Windows.Forms.ListView listView2;
    private System.Windows.Forms.ColumnHeader columnHeader6;
    private System.Windows.Forms.ColumnHeader columnHeader7;
    private System.Windows.Forms.ColumnHeader columnHeader8;
    private System.Windows.Forms.ColumnHeader columnHeader9;
    private System.Windows.Forms.ColumnHeader columnHeader10;
    private System.Windows.Forms.ColumnHeader columnHeader11;
    private System.Windows.Forms.ColumnHeader columnHeader12;
    private System.Windows.Forms.ColumnHeader columnHeader13;
    private System.Windows.Forms.ColumnHeader columnHeader14;
    private System.Windows.Forms.ColumnHeader columnHeader15;
    private System.Windows.Forms.ColumnHeader columnHeader16;
    private System.Windows.Forms.ColumnHeader columnHeader17;
    private System.Windows.Forms.ColumnHeader columnHeader18;
    private System.Windows.Forms.ColumnHeader columnHeader19;
    private System.Windows.Forms.Label label9;
    private MPNumericTextBox mpNumericTextBoxStartingAroundDeviation;
    private MPButton mpButtonSaveTemplate;
    private MPButton mpButtonApplyTemplate;
    private MPComboBox mpComboBoxTemplates;
    private System.Windows.Forms.ColumnHeader columnHeader1;
    private MPNumericTextBox mpNumericTextBoxPostRec;
    private MPNumericTextBox mpNumericTextBoxPreRec;
    private MPRadioButton radioOnAllChannels;
    private ListBox listBoxPrograms;
    private MPRadioButton mpRadioButtonInAllCategories;
  }
}
