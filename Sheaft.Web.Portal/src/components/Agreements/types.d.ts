import { DayOfWeek } from '$enums/days'

export type AgreementDeliveryForm = {
  deliveryDays: DayOfWeek[];
  limitOrderHourOffset: number;
  limitEnabled:boolean;
}
