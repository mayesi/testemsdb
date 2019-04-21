using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Demographics;

namespace SchedulingLib
{
	/// <brief>Class to store appointments and related information.  Provides validation and an interface to the I/O module</brief>
	/// <summary>
	/// This class throws exceptions on bad arguements for buisiness logic.  
	/// Assumes input level validation is done at the UI layer.
	/// Provides an overridden ToString that can convert an object to a stream for writing to a file
	/// </summary>
	public class Appointment
	{
		#region Data Members
		/// <summary>
		/// Appointment info
		/// </summary>
		public PatientInfo PatientHCN { get; private set; } /// Patient Health Card Number
		public PatientInfo CareGiverHCN { get; private set; } /// Care giver HCN
		public DateTime DateTime { get; private set; } /// Date and time of the appointment.  Setter throws exception when time given is not a defined constant
		public StatusValues Status { get; private set; } /// reflects if the object accurately represents an appointment in the database	
		public List<string> BillingCodes { get; set; }

		/// <summary>
		/// Constants, readonly, enums
		/// </summary>
		static private readonly int[] APPT_WEEKDAY_TIMES = new int[] { 9, 10, 11, 12, 1, 2 };
		static private readonly int[] APPT_WEEKEND_TIMES = new int[] { 10, 11 };
		public enum StatusValues { Available = 0, Booked, Cancelled }; /// Status of appointment
		private static readonly string keyFormat = "yyyy-MM-dd HH"; /// DateTime format
		public readonly int duration = 60; /// Length of appointment in MINUTES	
		#endregion

		#region Constructors
		public Appointment(DateTime dateAndTime, PatientInfo patient, StatusValues val)
		{
			DateTime = dateAndTime;
			Status = val;
			PatientHCN = patient;
			CareGiverHCN = null;
			BillingCodes = new List<string>();
		}

		public Appointment(DateTime dateAndTime, PatientInfo patient, PatientInfo careGiver, StatusValues val)
		{
			DateTime = dateAndTime;
			Status = val;
			PatientHCN = patient;
			CareGiverHCN = careGiver;
			BillingCodes = new List<string>();
		}

		public Appointment(DateTime dateAndTime)
		{
			try
			{
				DateTime = dateAndTime;

				// Use DAL to fill information, throw to handle no appointment slot
			}
			catch (Exception)
			{
				PatientHCN = null;
				CareGiverHCN = null;
				Status = StatusValues.Available;
				BillingCodes = new List<string>();
			}
		}

		#endregion

		#region General Methods
		/// <summary>
		/// Formats string for file I/O.  Takes all relevant data members and transforms them into key-value strings
		/// </summary>
		/// <returns>Formatted string</returns>
		public string[][] ToKeyValue()
		{
			/// Constants defined for code clarity
			const int NUM_VARS = 4;

			const int KEY_IDX = 0;
			const int VALUE_IDX = 1;

			const int PATIENT_IDX = 0;
			const int CAREG_IDX = 1;
			const int DATE_IDX = 2;
			const int STATUS_IDX = 3;

			string[][] EncodedString = new string[2][] {
				new string[NUM_VARS] { "", "", "", "" },
				new string[NUM_VARS] { "", "", "", "" }
				};

			try
			{
				EncodedString[KEY_IDX][PATIENT_IDX] = "patient";
				if (PatientHCN == null)
				{
					EncodedString[VALUE_IDX][PATIENT_IDX] = "";
				}
				else
				{
					EncodedString[VALUE_IDX][PATIENT_IDX] = PatientHCN.ToString();
				}

				EncodedString[KEY_IDX][CAREG_IDX] = "careGiver";
				if (CareGiverHCN == null)
				{
					EncodedString[VALUE_IDX][CAREG_IDX] = "";
				}
				else
				{
					EncodedString[VALUE_IDX][CAREG_IDX] = CareGiverHCN.ToString();
				}

				EncodedString[KEY_IDX][DATE_IDX] = "dateTime";
				EncodedString[VALUE_IDX][DATE_IDX] = DateTime.ToString(keyFormat);

				EncodedString[KEY_IDX][STATUS_IDX] = "status";
				EncodedString[VALUE_IDX][STATUS_IDX] = Status.ToString();
			}
			catch (Exception e)
			{
				throw e;
			}

			return EncodedString;
		}
		#endregion

		#region DAL Methods
		/// <summary>
		/// Accesses the schedule database and updates a slot with different information
		/// </summary>

		public static List<Appointment> GetDay(DateTime date)
		{
			date = DateSupport.ToHour(date, 1);

			List<Appointment> appointments = new List<Appointment>();
			int numAppts = -1;
			int dayOfWeek = DateSupport.GetDayIndex(date);

			if (dayOfWeek == (int)DayOfWeek.Saturday ||
				dayOfWeek == (int)DayOfWeek.Sunday)
			{
				numAppts = APPT_WEEKDAY_TIMES.Length;
			}
			else
			{
				numAppts = APPT_WEEKEND_TIMES.Length;
			}

			for (int i = 0; i < numAppts; i++)
			{
				appointments.Add(new Appointment(date));
				date = date.AddHours(1);
			}

			return appointments;
		}

		public static List<Appointment> GetWeek(DateTime date)
		{
			List<Appointment> appointments = new List<Appointment>();
			date = DateSupport.BackToSunday(date);

			// get all appointment slots from all days in the week
			for (int i = 0; i < 7; i++)
			{
				appointments.AddRange(GetDay(date));
				date = date.AddDays(1);
			}

			return appointments;
		}

		public static List<Appointment> GetMonth(DateTime date)
		{
			List<Appointment> appointments = new List<Appointment>();

			// prepare our for loop
			date = DateSupport.BackToMonthStart(date);
			int numDays = DateSupport.NumDaysInMonth(date);

			// get all appointment slots from all days in the month
			for (int i = 0; i < numDays; ++i)
			{
				appointments.AddRange(GetDay(date));
				date = date.AddDays(1);
			}

			return appointments;
		}

		public void InsertAppointment()
		{
			// Interface with DAL
			// throw if error occurs
		}

		public void UpdateStatus(StatusValues newStatus)
		{
			// Interface with DAL
			// throw if error occurs
		}

		#endregion
	}

	public static class DateSupport
	{
		public static DateTime BackToSunday(DateTime date)
		{
			int dayOfWeek = GetDayIndex(date);

			// go backwards until we hit Sunday
			while (dayOfWeek > 0)
			{
				date = date.AddDays(-1);
				--dayOfWeek;
			}

			return date;
		}

		public static DateTime BackToMonthStart(DateTime date)
		{
			int dayOfMonth = int.Parse(date.ToString("dd"));

			// go backwards until we hit Sunday
			while (dayOfMonth > 1)
			{
				date = date.AddDays(-1);
				--dayOfMonth;
			}

			return date;
		}

		public static int GetDayIndex(DateTime date)
		{
			string dayStr = date.ToString("dddd");
			int dayIndex = 0;

			if (dayStr.Equals("Sunday"))
			{
				dayIndex = 0;
			}
			else if (dayStr.Equals("Monday"))
			{
				dayIndex = 1;
			}
			else if (dayStr.Equals("Tuesday"))
			{
				dayIndex = 2;
			}
			else if (dayStr.Equals("Wednesday"))
			{
				dayIndex = 3;
			}
			else if (dayStr.Equals("Thursday"))
			{
				dayIndex = 4;
			}
			else if (dayStr.Equals("Friday"))
			{
				dayIndex = 5;
			}
			else if (dayStr.Equals("Saturday"))
			{
				dayIndex = 6;
			}
			else
			{
				throw new ArgumentException("The DateTime object has a weekday that doens't exist", "date");
			}

			return dayIndex;
		}

		public static DateTime ToHour(DateTime date, int toHour)
		{
			int hour = int.Parse(date.ToString("HH"));
			int minute = int.Parse(date.ToString("mm"));
			int second = int.Parse(date.ToString("ss"));

			int hourDelta = hour - toHour;
			int minuteDelta = minute - toHour;
			int secondDelta = second - toHour;

			date = date.AddHours(-hourDelta);
			date = date.AddMinutes(-minuteDelta);
			date = date.AddSeconds(-secondDelta);

			return date;
		}

		public static int NumDaysInMonth(DateTime date)
		{
			int monthIndex = int.Parse(date.ToString("MM"));
			int numDaysInMonth = 0;

			switch (monthIndex)
			{
				case 1:
					numDaysInMonth = 31;
					break;
				case 2:
					numDaysInMonth = 28;
					break;
				case 3:
					numDaysInMonth = 31;
					break;
				case 4:
					numDaysInMonth = 30;
					break;
				case 5:
					numDaysInMonth = 31;
					break;
				case 6:
					numDaysInMonth = 30;
					break;
				case 7:
					numDaysInMonth = 31;
					break;
				case 8:
					numDaysInMonth = 31;
					break;
				case 9:
					numDaysInMonth = 30;
					break;
				case 10:
					numDaysInMonth = 31;
					break;
				case 11:
					numDaysInMonth = 30;
					break;
				case 12:
					numDaysInMonth = 31;
					break;
				default:
					throw new ArgumentException("The date has a month that doesn't exist", "date");
			}

			return numDaysInMonth;
		}
	}
}