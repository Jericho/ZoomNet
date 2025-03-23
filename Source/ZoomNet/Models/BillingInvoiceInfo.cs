using System;
using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// Information regarding an invoice.
	/// </summary>
	public class BillingInvoiceInfo
	{
		/// <summary>
		/// Gets or sets the remaining balance of the invoice after all payments and refunds are applied.
		/// </summary>
		[JsonPropertyName("balance")]
		public decimal Balance { get; set; }

		/// <summary>
		/// Gets or sets the date by which the payment for this invoice is due.
		/// </summary>
		[JsonPropertyName("due_date")]
		public DateTime DueDate { get; set; }

		/// <summary>
		/// Gets or sets the unique identifier of the invoice.
		/// </summary>
		[JsonPropertyName("id")]
		public string Id { get; set; }

		/// <summary>
		/// Gets or sets the date when the invoice was generated.
		/// </summary>
		[JsonPropertyName("invoice_date")]
		public DateTime InvoiceDate { get; set; }

		/// <summary>
		/// Gets or sets the invoice number.
		/// </summary>
		[JsonPropertyName("invoice_number")]
		public string InvoiceNumber { get; set; }

		/// <summary>
		/// Gets or sets the status of the invoice.
		/// </summary>
		/// <remarks>
		/// The data type of this property should probably be an enum but the documentation does not list all the possible values.
		/// </remarks>
		[JsonPropertyName("status")]
		public string Status { get; set; }

		/// <summary>
		/// Gets or sets the date used to determine which charges are to be billed.
		/// </summary>
		/// <remarks>
		/// All charges that are to be billed on this date or prior will be included in the invoice.
		/// </remarks>
		[JsonPropertyName("target_date")]
		public DateTime TargetDate { get; set; }

		/// <summary>
		/// Gets or sets the tax amount.
		/// </summary>
		[JsonPropertyName("tax_amount")]
		public decimal TaxAmount { get; set; }

		/// <summary>
		/// Gets or sets the total invoice amount.
		/// </summary>
		[JsonPropertyName("total_amount")]
		public decimal TotalAmount { get; set; }
	}
}
