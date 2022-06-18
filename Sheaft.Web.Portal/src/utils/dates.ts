import { format, formatDistanceToNow } from "date-fns";
import { fr } from "date-fns/locale";

export const dateDistance = (date): string => {
  return formatDistanceToNow(new Date(date), { addSuffix: true, locale: fr });
};

export const dateStr = (date, pattern?: string | undefined): string => {
  return format(new Date(date), pattern ?? "dd/MM/yyyy", { locale: fr });
};
