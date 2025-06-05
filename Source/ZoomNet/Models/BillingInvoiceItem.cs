using System;
using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// Information regarding an invoice item.
	/// </summary>
	public class BillingInvoiceItem
	{
		/// <summary>
		/// Gets or sets the description of the charge.
		/// </summary>
		[JsonPropertyName("charge_name")]
		public string Description { get; set; }

		/// <summary>
		/// Gets or sets the identifier of the charge.
		/// </summary>
		[JsonPropertyName("charge_number")]
		public string ItemId { get; set; }

		/// <summary>
		/// Gets or sets the type of the charge.
		/// </summary>
		/// <remarks>Should probably be an ENUM, but the documentation does not list all the possible values.</remarks>
		[JsonPropertyName("charge_type")]
		public string Type { get; set; }

		/// <summary>
		/// Gets or sets the billing end date.
		/// </summary>
		[JsonPropertyName("end_date")]
		public DateTime EndDate { get; set; }

		/// <summary>
		/// Gets or sets the partner SKU.
		/// </summary>
		[JsonPropertyName("partner_sku")]
		public string PartnerSku { get; set; }

		/// <summary>
		/// Gets or sets the purchase order number.
		/// </summary>
		[JsonPropertyName("purchase_order_number")]
		public string PurchaseOrderNumber { get; set; }

		/// <summary>
		/// Gets or sets the quantity.
		/// </summary>
		[JsonPropertyName("quantity")]
		public int Quantity { get; set; }

		/// <summary>
		/// Gets or sets the billing start date.
		/// </summary>
		[JsonPropertyName("start_date")]
		public DateTime StartDate { get; set; }

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
