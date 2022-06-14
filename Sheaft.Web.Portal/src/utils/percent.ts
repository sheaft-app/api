export const percent = value => {
  let formatter = new Intl.NumberFormat("fr-FR", {
    style: "percent",
    minimumSignificantDigits: 2
  });

  if (value <= 1) return formatter.format(value);

  return formatter.format(value / 100);
};
