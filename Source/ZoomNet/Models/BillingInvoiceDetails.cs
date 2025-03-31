using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// Detailed information regarding an invoice.
	/// </summary>
	public class BillingInvoiceDetails : BillingInvoiceSummary
	{
		/// <summary>
		/// Gets or sets the invoice items.
		/// </summary>
		[JsonPropertyName("invoice_items")]
		public BillingInvoiceItem[] Items { get; set; }
	}
}
